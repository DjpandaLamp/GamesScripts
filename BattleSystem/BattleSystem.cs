using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

public class BattleSystem : MonoBehaviour
{
    public GameObject baseMenu;
    public TypeWriter baseMenuFlavorText;
    public GameObject[] baseMenuButtons;
    public GameObject TextArrow;
    public GameObject skillMenu;


    public battleStateMachine state;
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    public GameObject visualCanvasElement;

    public GameObject[] PlayerArray;
    public GameObject[] EnemyArray;
    public GameObject[] PlayerCurrentArray;
    public GameObject[] EnemyCurrentArray;


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

    public string[] SkillsDescs;


    // Start is called before the first frame update
    void Start()
    {
        SkillsDescs = new string[20]; 

        PlayerArray = new GameObject[PlayerCount];
        EnemyArray = new GameObject[EnemyCount];

        BasePlayerCount = PlayerCount;
        BaseEnemyCount = EnemyCount;

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

        currentActiveAgent = PlayerArray[0];

        yield return new WaitForSeconds(1);

        state = battleStateMachine.PlayerTurn;
        Debug.Log("Battle Loaded");
        PlayerTurn();


    }
    void LoadPlayerUnit()
    {
        for (int i = 0; i < PlayerCount; i++)
        {
            GameObject NewPlayer = Instantiate(PlayerPrefab, PlayerTransformArray[i].position, Quaternion.identity, visualCanvasElement.transform);
            BattleAgent battleAgent = NewPlayer.GetComponent<BattleAgent>();
            battleAgent.agentCount = i;
            battleAgent.agentId = 1;
            PlayerArray[i] = NewPlayer;
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

        }

    }
    void PlayerTurn()
    {
        StatusUpdate();
        targetedAgent = null;
        StartCoroutine(AgentScale(currentActiveAgent, new Vector3(1f, 1f, 1f)));
        baseMenuFlavorText.fullText = "What's Your Move?";
    }

    void StatusUpdate()
    {
        BattleAgent agent = currentActiveAgent.GetComponent<BattleAgent>();
        agent.isDefending = false;
    }

    public void OnAttackButton()
    {
        Debug.Log("AttackButtonPressed");
        if (state != battleStateMachine.PlayerTurn)
        {
            return;
        }
        StartCoroutine(BasicAttack(PlayerArray[0].GetComponent<BattleAgent>(), true));
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
            StartCoroutine(BasicHeal(activeAgent));
        }
    }
    public void OnDefenseButton()
    {
        StartCoroutine(OnDefenseEnumberable());
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
            EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBoxImage.color = EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBaseColor;
            dir = false;
        }
        if (Input.GetKeyDown("w") || Input.GetKeyDown("up"))
        {
            playerCursorPos -= 1;
            EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBoxImage.color = EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBaseColor;
            dir = true;
        }

        if (EnemyArray.Length-1 < playerCursorPos)
        {
            playerCursorPos = 0;
            EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBoxImage.color = EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBaseColor;
        }
        if (playerCursorPos < 0)
        {
            playerCursorPos = EnemyArray.Length-1;
            EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBoxImage.color = EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBaseColor;
        }

        while (EnemyArray[playerCursorPos].activeSelf == false)
        {
            if (dir == true)
            {
                playerCursorPos -= 1;
                EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBoxImage.color = EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBaseColor;
            }
            else
            {
                playerCursorPos += 1;
                EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBoxImage.color = EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBaseColor;
            }
            if (EnemyArray.Length - 1 < playerCursorPos)
            {
                playerCursorPos = 0;
                EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBoxImage.color = EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBaseColor;
            }
            if (playerCursorPos < 0)
            {
                playerCursorPos = EnemyArray.Length - 1;
                EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBoxImage.color = EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentBaseColor;
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


        //Temp code, replace with enemy picker in near future//

        if (attackDirection == true)
        {
            if (targetedAgent == null)
            {
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
            }

            
            /*
            targetedAgent = EnemyArray[Random.Range(0, BaseEnemyCount)].GetComponent<BattleAgent>();
            while (targetedAgent.gameObject.activeSelf == false) 
            {
                targetedAgent = EnemyArray[Random.Range(0, BaseEnemyCount)].GetComponent<BattleAgent>();
            }*/
        }


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
                            ChangeCurrentAgent();
                        }
                    }
                }
            }
            else
            {

                baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                yield return StartCoroutine(TextPrinterWait(0));
                ChangeCurrentAgent();
            }
        }
        //Enemy Attack
        if (attackDirection == false)
        {

            state = battleStateMachine.Text;
            if (isDefeated)
            {
                PlayerCount = BasePlayerCount;
                PlayerCount -= 1;
                for (int i = 0; i < PlayerArray.Length; i++)
                {

                    if (PlayerArray[i].activeSelf == false)
                    {
                        PlayerCount -= 1;
                    }

                    if (i == PlayerArray.Length - 1)
                    {
                        if (PlayerCount == 0)
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

                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                            yield return StartCoroutine(TextPrinterWait(0));
                            targetedAgent.gameObject.SetActive(false);
                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " was Defeated.";
                            yield return StartCoroutine(TextPrinterWait(0));
                            ChangeCurrentAgent();
                        }
                    }
                }
            }
            else
            {

                baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                yield return StartCoroutine(TextPrinterWait(0));

                ChangeCurrentAgent();
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
        ChangeCurrentAgent();
    }


    IEnumerator EnemyTurn(int enemyIndex)
    {
        
        EnemyArray[enemyIndex].GetComponent<BattleAgent>().agentHasGone = true;
        if (EnemyArray[enemyIndex].activeSelf == true)
        {

            //Enemy Basic Attack
            #region
            StartCoroutine(AgentScale(currentActiveAgent,new Vector3(1f, 1f, 1f)));
            string agentName = "";
            agentName = EnemyArray[enemyIndex].GetComponent<BattleAgent>().agentName;
            baseMenuFlavorText.fullText = agentName + " attacks!";
            yield return StartCoroutine(TextPrinterWait(0));
            
            targetedAgent = PlayerArray[Random.Range(0,2)].GetComponent<BattleAgent>();
            while (targetedAgent.gameObject.activeSelf == false)
            {
                targetedAgent = PlayerArray[Random.Range(0, 2)].GetComponent<BattleAgent>();
            }
            StartCoroutine(BasicAttack(EnemyArray[enemyIndex].GetComponent<BattleAgent>(), false));
            #endregion
        }
        else
        {
            ChangeCurrentAgent();
        }
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
        for (int i = 0; i < PlayerCount; i++)
        {
            if (currentActiveAgent == PlayerArray[i])
            {

                if (PlayerCount > i + 1)
                {
                    currentActiveAgent = PlayerArray[i + 1];
                    state = battleStateMachine.PlayerTurn;
                    PlayerTurn();
                    Debug.Log("PlayerTurnToPlayerTurn");
                    return;

                }
                else
                {
                    currentActiveAgent = EnemyArray[0];
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
                state = battleStateMachine.PlayerTurn;
                PlayerTurn();
                Debug.Log("EnemyTurnToPlayerTurn");
                return;
            }
            if (EnemyArray[i].GetComponent<BattleAgent>().agentHasGone == true || EnemyArray[i].activeSelf == false)
            {
                currentActiveAgent = EnemyArray[i];
                continue;
            }
            else
            {
                currentActiveAgent = EnemyArray[i];
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

    IEnumerator EndBattle()
    {
        if (state == battleStateMachine.Win)
        {
            baseMenuFlavorText.fullText = "YOU WIN!!!!";
            yield return StartCoroutine(TextPrinterWait(0));
            //return to overworld
            yield return new WaitForSeconds(3);
            SceneManager.LoadSceneAsync(0);
        }
        if (state == battleStateMachine.Lose)
        {
            baseMenuFlavorText.fullText = "You have lost. Regret your actions and try again";
            yield return StartCoroutine(TextPrinterWait(0));
            //give gameover screen
            yield return new WaitForSeconds(3);
            SceneManager.LoadSceneAsync(0);
        }
        if (state == battleStateMachine.Flee)
        {
            baseMenuFlavorText.fullText = "You have fleed. Regret your actions and try again";
            yield return StartCoroutine(TextPrinterWait(0));
            //give gameover screen
            yield return new WaitForSeconds(3);
            SceneManager.LoadSceneAsync(0);
        }
    }
        
}




