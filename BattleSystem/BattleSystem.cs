using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public enum battleStateMachine
{
    Start,
    PlayerTurn,
    EnemyTurn,
    Text,
    PlayerTargeting,
    PlayerSkill,
    Win,
    Lose,
    Flee
}

public class MoveInfo : MonoBehaviour
{
    public string move;
    public GameObject attackingAgent;
    public BattleAgent targetedAgent;
}


public class BattleSystem : MonoBehaviour
{
    public GameObject baseMenu;
    public TypeWriter baseMenuFlavorText;
    public GameObject[] baseMenuButtons;
    public GameObject TextArrow;
    public GameObject skillMenu;

    public JSONSave JSONSave;
    public battleStateMachine state;
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    public GameObject visualCanvasElement;

    public List<GameObject> PlayerArray;
    public GameObject[] EnemyArray;
    public GameObject[] PlayerCurrentArray;
    public GameObject[] EnemyCurrentArray;

    public MoveInfo[] moves; //contains the data for moves.
    public GameObject movesContainer;
    public List<BattleAgent> AgentOrder; //calculated at the start of each turn.
    public Image[] battleImages; //Images sorted
    public int removedAgentCount;

    public int playerCursorPos;
    public bool isBackPressed;
    


    public Transform[] PlayerTransformArray;
    public Transform[] EnemyTransformArray;

    public GameObject currentActiveAgent;
    public BattleAgent targetedAgent;
    public float targetColorBlend;

    public int BasePlayerCount;
    public int BaseEnemyCount;
    public int PlayerCount;
    public int EnemyCount; //Number of Enemies to be Generated
    public int currentActiveAgentInt;
    public int targetedAgentNumber;
    

    public SceneLoader sceneLoader;

    public string[] SkillsDescs;


    // Start is called before the first frame update
    void Start()
    {
        SkillsDescs = new string[20]; 

        
        EnemyArray = new GameObject[EnemyCount];

        BasePlayerCount = PlayerCount;
        BaseEnemyCount = EnemyCount;
        moves = new MoveInfo[PlayerCount + EnemyCount];
        JSONSave = GameObject.Find("PersistantObject").GetComponent<JSONSave>();
        sceneLoader = GameObject.Find("PersistantObject").GetComponent<SceneLoader>();

        TextArrow.gameObject.SetActive(false);

        state = battleStateMachine.Start;
        StartCoroutine(BattleInitalize());

        SkillsDescs[0] = "THIS IS A DEBUG STRING. IF YOU ARE SEEING THIS, SOMETHING HAS GONE HORRIBLY WRONG"; //Error String
        SkillsDescs[1] = "Through the power of self medication, you heal yourself by 40% of your Energy Attack."; //Basic Heal
        SkillsDescs[2] = "You should thank your mom for those martal arts classes. You raise your defense by 1.25 times for this turn."; //Basic Defense

    }

    IEnumerator BattleInitalize()
    {
        LoadPlayerUnit();
        LoadEnemyUnit();

        currentActiveAgent = EnemyArray[0];
        yield return new WaitForEndOfFrame();
        CalculateSpeed();
        yield return new WaitForEndOfFrame();

        state = battleStateMachine.EnemyTurn;
        Debug.Log("Battle Loaded");
        StartCoroutine(EnemyTurn(0));


    }
    void LoadPlayerUnit()
    {
        for (int i = 0; i < PlayerCount; i++)
        {
            GameObject NewPlayer = Instantiate(PlayerPrefab, PlayerTransformArray[i].position, Quaternion.identity, visualCanvasElement.transform);
            BattleAgent battleAgent = NewPlayer.GetComponent<BattleAgent>();
            battleAgent.agentCount = i;
            battleAgent.agentId = i+1;
            PlayerArray.Add(NewPlayer);
           AgentOrder.Add(NewPlayer.GetComponent<BattleAgent>());
        }
    }
    void LoadEnemyUnit()
    {
        for (int i = 0; i < EnemyCount; i++)
        {
            GameObject NewEnemy = Instantiate(EnemyPrefab, EnemyTransformArray[i].position, Quaternion.identity, visualCanvasElement.transform);
            BattleAgent battleAgent = NewEnemy.GetComponent<BattleAgent>();
            battleAgent.agentCount = i;
            battleAgent.agentId = 0;

            EnemyArray[i] = NewEnemy;
           AgentOrder.Add(NewEnemy.GetComponent<BattleAgent>());
        }

    }

    void CalculateSpeed()
    {
        AgentOrder.Sort();
        for (int i = 0; i < battleImages.Length; i++)
        {

            battleImages[i].sprite = null;
        }

        for (int i = 0; i < AgentOrder.ToArray().Length; i++)
        {
            AgentOrder[i].currentBattleSpeedIndex = i;
            battleImages[i].sprite = AgentOrder[i].agentImage.sprite;
        }
    }

    void PlayerTurn()
    {
        StatusUpdate();
        targetedAgent = null;
        StartCoroutine(AgentScale(currentActiveAgent, new Vector3(1f, 1f, 1f)));
        baseMenuFlavorText.fullText = "What's Your Move?";
    }

    IEnumerator EnemyTurn(int enemyIndex)
    {

        EnemyArray[enemyIndex].GetComponent<BattleAgent>().agentHasGone = true;
        if (EnemyArray[enemyIndex].activeSelf == true)
        {
            
            //Enemy Basic Attack
            #region
            StartCoroutine(AgentScale(currentActiveAgent, new Vector3(1f, 1f, 1f)));
            string agentName = "";
            agentName = EnemyArray[enemyIndex].GetComponent<BattleAgent>().agentName;
            baseMenuFlavorText.fullText = agentName + " attacks!";
            yield return StartCoroutine(TextPrinterWait(0));

            targetedAgentNumber = UnityEngine.Random.Range(0, PlayerArray.ToArray().Length);
            targetedAgent = PlayerArray[targetedAgentNumber].GetComponent<BattleAgent>();
            while (targetedAgent.gameObject.activeSelf == false)
            {
                targetedAgentNumber = UnityEngine.Random.Range(0, PlayerArray.ToArray().Length);
                targetedAgent = PlayerArray[targetedAgentNumber].GetComponent<BattleAgent>();
            }
            MoveInfo move = movesContainer.AddComponent<MoveInfo>();
            move.move = "Attack";
            move.attackingAgent = EnemyArray[enemyIndex];
            move.targetedAgent = targetedAgent;
            moves[EnemyArray[enemyIndex].GetComponent<BattleAgent>().currentBattleSpeedIndex] = move;
            ChangeCurrentAgent();
            //StartCoroutine(BasicAttack(EnemyArray[enemyIndex].GetComponent<BattleAgent>(), false));
            #endregion
        }
        else
        {
            ChangeCurrentAgent();
        }
    }


    void StatusUpdate()
    {
        BattleAgent agent = currentActiveAgent.GetComponent<BattleAgent>();
        agent.isDefending = false;
    }

    public void OnAttackButton()
    {
        StartCoroutine(AttackButtonPlayer());
    }

    public IEnumerator AttackButtonPlayer()
    {
        Debug.Log("AttackButtonPressed");
        if (state != battleStateMachine.PlayerTurn && state != battleStateMachine.PlayerTargeting)
        {
            yield return 0;
        }
        
        state = battleStateMachine.PlayerTargeting;
            baseMenuFlavorText.fullText = "Pick your Target.";
            yield return StartCoroutine(TextPrinterWait(1));
            while (targetedAgent == null)
            {
                if (isBackPressed)
                {
                    isBackPressed = false;
                    state = battleStateMachine.PlayerTurn;
                    PlayerTurn();
                    yield break;
                }

                yield return StartCoroutine(PlayerTargeting());
            }

        MoveInfo move = movesContainer.AddComponent<MoveInfo>();
        move.move = "Attack";
        move.attackingAgent = currentActiveAgent;
        move.targetedAgent = targetedAgent;
        moves[currentActiveAgent.GetComponent<BattleAgent>().currentBattleSpeedIndex] = move;
        ChangeCurrentAgent();
        //StartCoroutine(BasicAttack(currentActiveAgent.GetComponent<BattleAgent>(), true));
    }

    public void OnFleeButton()
    {
        if (state != battleStateMachine.PlayerTurn)
        {
            return;
        }
        state = battleStateMachine.Flee;
        StartCoroutine(EndBattle());
    }

    public void OnSkillButton()
    {
        
        state = battleStateMachine.PlayerSkill;
    }

    public void OnBackButton()
    {
        isBackPressed = true;
    }

    public void OnHealSkillsButton(int grade)
    {
        BattleAgent activeAgent = currentActiveAgent.GetComponent<BattleAgent>();
        if (grade == 0)
        {
            if (activeAgent.agentENCurrent < 10)
            {
                baseMenuFlavorText.fullText = activeAgent.agentName.ToString() + " doesn't have enough energy!";
                return;
            }
            activeAgent.agentENCurrent -= 10;
            targetedAgent = activeAgent;
            //todo: add targeting system for heals;

            MoveInfo move = movesContainer.AddComponent<MoveInfo>();
            move.move = "Heal" + grade.ToString();
            move.attackingAgent = currentActiveAgent;
            move.targetedAgent = activeAgent;
            moves[currentActiveAgent.GetComponent<BattleAgent>().currentBattleSpeedIndex] = move;

            ChangeCurrentAgent();
            //StartCoroutine(BasicHeal(activeAgent));
        }
    }
    public void OnDefenseButton()
    {
        MoveInfo move = movesContainer.AddComponent<MoveInfo>();
        move.move = "Defend";
        move.attackingAgent = currentActiveAgent;
        move.targetedAgent = move.attackingAgent.GetComponent<BattleAgent>();
        moves[currentActiveAgent.GetComponent<BattleAgent>().currentBattleSpeedIndex] = move;
        ChangeCurrentAgent();
        //StartCoroutine(OnDefenseEnumberable());
    }



    public IEnumerator OnDefenseEnumberable()
    {
        BattleAgent agent = currentActiveAgent.GetComponent<BattleAgent>();
        agent.isDefending = true;
        baseMenuFlavorText.fullText = agent.agentName + " Defended.";
        yield return StartCoroutine(TextPrinterWait(0));
        ChangeCurrentAgent();
    }

    IEnumerator PlayerTargeting()
    {
        bool dir = false;
        if (Input.GetKeyDown("s") || Input.GetKeyDown("down"))
        {
            playerCursorPos += 1;
            dir = false;
        }
        if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
        {
            playerCursorPos -= 1;
            dir = true;
        }

        if (EnemyArray.Length-1 < playerCursorPos)
        {
            playerCursorPos = 0;
        }
        if (playerCursorPos < 0)
        {
            playerCursorPos = EnemyArray.Length-1;
        }

        while (EnemyArray[playerCursorPos].activeSelf == false)
        {
            if (dir == true)
            {
                playerCursorPos -= 1;
            }
            else
            {
                playerCursorPos += 1;
            }
            if (EnemyArray.Length - 1 < playerCursorPos)
            {
                playerCursorPos = 0;
            }
            if (playerCursorPos < 0)
            {
                playerCursorPos = EnemyArray.Length - 1;
            }

        }
        
        EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBoxImage.color = ColorTarget();

        if (Input.GetKeyDown("z") && EnemyArray[playerCursorPos].activeSelf == true)
        {
            targetedAgent = EnemyArray[playerCursorPos].GetComponent<BattleAgent>();
        }

        if (targetedAgent == null)
        {
            yield return null;
        }
        EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBoxImage.color = EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBaseColor;
        yield break;
    }

    Color ColorTarget()
    {
        targetColorBlend = Mathf.Clamp01( 0.6f+Mathf.Sin(Time.time) / 2);
        Color baseColor = EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBaseColor;
        Color blend = new Color(1, 1, 0, 1);
        Color blendColor = (Color.Lerp(baseColor, blend, 0.5f+Mathf.PingPong(Time.time,0.5f)));
        return blendColor;
    }

    IEnumerator BasicAttack(BattleAgent attackingAgent, bool attackDirection)
    {
        Debug.Log("Attack");
        if (attackDirection)
        {
            Debug.Log("PlayerAttack");
        }
        else
        {
            Debug.Log("EnemyAttack");
        }
        //attackDirection = true means player is attacking
        //attackDirection = false means Enemy is attacking



        Debug.Log("AgentTargeted: " + targetedAgent.agentName);

        int agentPreDamageHealth = targetedAgent.agentHPCurrent;

        bool isDefeated = targetedAgent.TakeDamage(attackingAgent.agentATKFull);

        //Player Attack
        if (attackDirection == true)
        {

            state = battleStateMachine.Text;
            if (isDefeated)
            {
                
                EnemyCount = BaseEnemyCount;
                EnemyCount -= 1;
                AgentOrder.Remove(targetedAgent);
                removedAgentCount += 1;
                for (int i = 0; i < EnemyArray.Length; i++)
                {

                    if (EnemyArray[i].activeSelf == false)
                    {
                        EnemyCount -= 1;
                    }
                    if (i == EnemyArray.Length-1)
                    {
                        if (EnemyCount == 0)
                        {
                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                            yield return StartCoroutine(TextPrinterWait(0));
                            targetedAgent.gameObject.SetActive(false);
                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " was Defeated.";
                            yield return StartCoroutine(TextPrinterWait(0));
                            state = battleStateMachine.Win;
                            StartCoroutine(EndBattle());
                        }
                        else
                        {
                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                            yield return StartCoroutine(TextPrinterWait(0));
                            targetedAgent.gameObject.SetActive(false);
                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " was Defeated.";
                            yield return StartCoroutine(TextPrinterWait(0));
                            
                        }
                    }
                }
            }
            else
            {

                baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                yield return StartCoroutine(TextPrinterWait(0));
                
            }
        }
        //Enemy Attack
        if (attackDirection == false)
        {

            state = battleStateMachine.Text;
            if (isDefeated)
            {
                PlayerArray.Remove(PlayerArray[targetedAgentNumber]);
                AgentOrder.Remove(PlayerArray[targetedAgentNumber].GetComponent<BattleAgent>());
                removedAgentCount += 1;
                PlayerCount = PlayerArray.ToArray().Length;
                    if (PlayerArray.ToArray().Length == 0)
                        {

                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                            yield return StartCoroutine(TextPrinterWait(0));
                            targetedAgent.gameObject.SetActive(false);
                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " was Defeated.";
                            yield return StartCoroutine(TextPrinterWait(0));
                            state = battleStateMachine.Lose;
                            StartCoroutine(EndBattle());
                        }
                        else
                        {
                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                            yield return StartCoroutine(TextPrinterWait(0));
                            targetedAgent.gameObject.SetActive(false);
                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " was Defeated.";
                            yield return StartCoroutine(TextPrinterWait(0));
                            
                        }   
            }
            else
            {

                baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                yield return StartCoroutine(TextPrinterWait(0));

                
            }
        }

        //continue to next agent turn or play death animation(s)
    }


    IEnumerator BasicHeal(BattleAgent attackingAgent)
    {
        state = battleStateMachine.Text;
        int agentPreDamageHealth = targetedAgent.agentHPCurrent;
        targetedAgent.ReceiveHeal(attackingAgent.agentEATKFull);
        if (agentPreDamageHealth - targetedAgent.agentHPCurrent == 0)
        {
            baseMenuFlavorText.fullText = "But the healing had no effect!";
        }
        else
        {
            state = battleStateMachine.Text;
            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " is healed " + Mathf.Abs(agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " Health!";
        }
        
        yield return StartCoroutine(TextPrinterWait(0));
        
    }


    

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            baseMenuButtons[i].gameObject.GetComponent<Button>().interactable = true;
        }
        if (state == battleStateMachine.PlayerTurn)
        {
            for (int i = 0; i < 4; i++)
            {
                baseMenuButtons[i].gameObject.SetActive(true);
            }
            baseMenuButtons[4].gameObject.SetActive(false);
        }
        if (state == battleStateMachine.PlayerTargeting)
        {
            for (int i = 0; i < 4; i++)
            {
                baseMenuButtons[i].gameObject.SetActive(false);
            }
            baseMenuButtons[4].gameObject.SetActive(true);
        }
        if (state == battleStateMachine.PlayerSkill)
        {
            if (isBackPressed)
            {
                isBackPressed = false;
                state = battleStateMachine.PlayerTurn;
            }
            for (int i = 0; i < 4; i++)
            {
                baseMenuButtons[i].gameObject.GetComponent<Button>().interactable = false;
            }
            skillMenu.SetActive(true);
        }
        else
        {
            skillMenu.SetActive(false);
        }
        if (state != battleStateMachine.PlayerTurn && state != battleStateMachine.PlayerTargeting && state != battleStateMachine.PlayerSkill)
        {
            for (int i = 0; i < baseMenuButtons.Length; i++)
            {
                baseMenuButtons[i].gameObject.SetActive(false);
                skillMenu.gameObject.SetActive(false);
            }
        }
    }

    void ChangeCurrentAgent()
    {
        StartCoroutine(AgentScale(currentActiveAgent,new Vector3(0.9f,0.9f,.9f)));
        state = battleStateMachine.Text;

        if (currentActiveAgentInt >= AgentOrder.ToArray().Length - 1)
        {
            StartCoroutine(ExecuteTurn());
            return;
        }

        for (int i = 0; i < PlayerCount; i++)
        {
            if (currentActiveAgent == PlayerArray[i])
            {
                if (PlayerCount > i + 1)
                {
                    currentActiveAgent = PlayerArray[i + 1];
                    currentActiveAgentInt = i + 5; 
                    state = battleStateMachine.PlayerTurn;
                    PlayerTurn();
                    Debug.Log("PlayerTurnToPlayerTurn");
                    return;
                }
                else
                {
                    currentActiveAgent = EnemyArray[0];
                    currentActiveAgentInt = 0;
                    state = battleStateMachine.EnemyTurn;
                    StartCoroutine(EnemyTurn(0));
                    Debug.Log("PlayerTurnToEnemyTurn");
                    return;
                }
                
            }
        }

        for (int i = 0; i <= EnemyArray.Length; i++)
        {
            Debug.Log(i.ToString());
            if (i == EnemyArray.Length)
            {
                for (int j = 0; j < EnemyArray.Length; j++)
                {
                    
                    EnemyArray[j].GetComponent<BattleAgent>().agentHasGone = false;
                }
                currentActiveAgent = PlayerArray[0];
                currentActiveAgentInt = 4;
                state = battleStateMachine.PlayerTurn;
                PlayerTurn();
                Debug.Log("EnemyTurnToPlayerTurn");
                return;
            }
            if (EnemyArray[i].GetComponent<BattleAgent>().agentHasGone == true || EnemyArray[i].activeSelf == false)
            {
                currentActiveAgent = EnemyArray[i];
                currentActiveAgentInt = i;
                continue;
            }
            else
            {
                currentActiveAgent = EnemyArray[i];
                currentActiveAgentInt = i;
                state = battleStateMachine.EnemyTurn;
                StartCoroutine(EnemyTurn(i));
                Debug.Log("EnemyTurnToEnemyTurn");
                return;
            }
        }
    }

    IEnumerator TextPrinterWait(int type)
    {
        if (type == 0)
        {
            baseMenuFlavorText.isFinished = true;
            yield return new WaitForSeconds(0.01f);
            while (baseMenuFlavorText.isFinished == false)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
            while (Input.GetMouseButtonDown(0) != true && Input.GetKeyDown("z") !=true)
            {
                TextArrow.gameObject.SetActive(true);
                yield return null;
            }
            TextArrow.gameObject.SetActive(false);
        }
        if (type == 1)
        {

        }

    }

    IEnumerator AgentScale(GameObject agent, Vector3 desiredScale)
    {
        
        while (Mathf.Abs(agent.transform.localScale.x - desiredScale.x) > 0.01f)
        {
            agent.transform.localScale = Vector3.Lerp(agent.transform.localScale, desiredScale, 5f * Time.deltaTime);
            yield return null;
        }
        
    }

    IEnumerator ExecuteTurn()
    {
        for (int i = 0; i < moves.Length; i++)
        {
            if (moves[i] == null)
            {
                Debug.Log("Null Move");
                continue; 
            }
            if (moves[i].move == "Attack")
            {
                Debug.Log("Attack Move");
                currentActiveAgent = moves[i].attackingAgent;
                targetedAgent = moves[i].targetedAgent;
                yield return StartCoroutine(BasicAttack(currentActiveAgent.GetComponent<BattleAgent>(), true));
                continue;
            }
            if (moves[i].move == "Defend")
            {
                Debug.Log("Defend Move");
                continue;
            }
            if (moves[i].move == "Heal")
            {

                currentActiveAgent = moves[i].attackingAgent;
                targetedAgent = moves[i].targetedAgent;
                yield return StartCoroutine(BasicHeal(moves[i].targetedAgent));
                continue;
            }
        }
        for (int i = 0; i < moves.Length; i++)
        {
            moves[i] = null;
            currentActiveAgentInt = 0;
        }
        CalculateSpeed();
        ChangeCurrentAgent(); 
        yield return null;
    }



    IEnumerator EndBattle()
    {
        if (state == battleStateMachine.Win)
        {
            baseMenuFlavorText.fullText = "YOU WIN!!!!";
            yield return StartCoroutine(TextPrinterWait(0));
            //return to overworld
            yield return new WaitForSeconds(3);
            JSONSave.LoadFromJSON(1, 0);
        }
        if (state == battleStateMachine.Lose)
        {
            baseMenuFlavorText.fullText = "You have lost. Regret your actions and try again";
            yield return StartCoroutine(TextPrinterWait(0));
            //give gameover screen
            yield return new WaitForSeconds(3);
            sceneLoader.SceneLoaded(0);
        }
        if (state == battleStateMachine.Flee)
        {
            baseMenuFlavorText.fullText = "You have fleed. Regret your actions and try again";
            yield return StartCoroutine(TextPrinterWait(0));
            //give gameover screen
            yield return new WaitForSeconds(3);
            JSONSave.LoadFromJSON(1, 0);
        }
    }
        
}




