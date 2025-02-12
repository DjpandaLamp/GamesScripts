using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Mathfunctions;

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
    public GameObject AgentOrderBox;

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
    private int moveIndex;
    public List<BattleAgent> AgentOrder; //calculated at the start of each turn.
    public int startingAgentOrderSize;
    public Image[] battleImages; //Images sorted
    public int removedAgentCount;

    public int playerCursorPos;
    public bool isBackPressed;

    private Coroutine activeAgentOrderBoxCoroutine;

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

    public BattleOverlayAnimationScript BattleOverlayAnimationScript;

    public bool turnEND = false;

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
        yield return StartCoroutine(LoadEnemyUnit());


        currentActiveAgent = EnemyArray[0];
        yield return new WaitForEndOfFrame();
        CalculateSpeed();
        startingAgentOrderSize = AgentOrder.ToArray().Length;
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
            battleAgent.agentIdentity = true;
            battleAgent.agentCount = i;
            battleAgent.agentId = i+1;
            PlayerArray.Add(NewPlayer);
           AgentOrder.Add(NewPlayer.GetComponent<BattleAgent>());
        }
    }
    IEnumerator LoadEnemyUnit()
    {
        int[] nameInts = new int[EnemyCount];
        for (int i = 0; i < EnemyCount; i++)
        {

            GameObject NewEnemy = Instantiate(EnemyPrefab, EnemyTransformArray[i].position, Quaternion.identity, visualCanvasElement.transform);
            BattleAgent battleAgent = NewEnemy.GetComponent<BattleAgent>();
            battleAgent.agentIdentity = false;
            battleAgent.agentCount = i;
            battleAgent.agentId = 0;

            EnemyArray[i] = NewEnemy;
            yield return new WaitForSeconds(0.01f);
            for (int j = 0; j < EnemyCount; j++)
            {
                if (EnemyArray[j] == null || i == j)
                {
                    continue;
                }
                if (EnemyArray[j].GetComponent<BattleAgent>().agentName == battleAgent.agentName)
                {
                    Debug.Log("work");
                    nameInts[j] += 1; 
                }
            }
           AgentOrder.Add(NewEnemy.GetComponent<BattleAgent>());
        }
        for (int i = 0; i < EnemyCount; i++)
        {
            string battleName = EnemyArray[i].GetComponent<BattleAgent>().agentName;
            switch (nameInts[i])
            {
                case 0:
                    battleName = battleName + " D";
                    EnemyArray[i].GetComponent<BattleAgent>().agentName = battleName;
                    
                    continue;
                case 1:
                    battleName = battleName + " C";
                    EnemyArray[i].GetComponent<BattleAgent>().agentName = battleName;
                    continue;
                case 2:
                    battleName = battleName + " B";
                    EnemyArray[i].GetComponent<BattleAgent>().agentName = battleName;
                    continue;
                case 3:
                    battleName = battleName + " A";
                    EnemyArray[i].GetComponent<BattleAgent>().agentName = battleName;
                    continue;
            }
        }
        yield return null;
    }

    void CalculateSpeed()
    {
        AgentOrder.Sort();
        moves = new MoveInfo[PlayerCount + EnemyCount];

        int _n = 20;
        int _y = -362;
        RectTransform rect = AgentOrderBox.GetComponent<RectTransform>();

        for (int i = 0; i < battleImages.Length; i++)
        {
            battleImages[i].gameObject.SetActive(true);
            battleImages[i].sprite = null;
        }

        for (int i = 0; i < AgentOrder.ToArray().Length; i++)
        {
            AgentOrder[i].currentBattleSpeedIndex = i;
            battleImages[i].sprite = AgentOrder[i].agentImage.sprite;
        }
        for (int i = 0; i < battleImages.Length; i++)
        {
            if (battleImages[i].sprite == null)
            {
                battleImages[i].gameObject.SetActive(false);
            }
            else
            {
                _n += 70;
                
            }
            
        }
        if (activeAgentOrderBoxCoroutine != null)
        {
            StopCoroutine(activeAgentOrderBoxCoroutine);
        }
        StartCoroutine(PrivMath.Vector2LerpRect(rect, new Vector2(rect.sizeDelta.x, _n), 0.05f, 100, 0));
        activeAgentOrderBoxCoroutine = StartCoroutine(PrivMath.Vector2LerpRect(rect, new Vector2(rect.localPosition.x, _y), 0.05f, 100, 1));
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
            //StartCoroutine(AgentScale(currentActiveAgent, new Vector3(1f, 1f, 1f)));
            string agentName = "";
            agentName = EnemyArray[enemyIndex].GetComponent<BattleAgent>().agentName;
            //baseMenuFlavorText.fullText = agentName + " attacks!";
            //yield return StartCoroutine(TextPrinterWait(0));
            yield return new WaitForSeconds(0.01f);
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
        if (Input.GetKeyDown("d") || Input.GetKeyDown("right"))
        {
            playerCursorPos += 1;
            dir = false;
        }
        if (Input.GetKeyDown("a") || Input.GetKeyDown("left"))
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
        string agentName = attackingAgent.agentName;
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
            baseMenuFlavorText.fullText = agentName + " attacks!";
            yield return StartCoroutine(TextPrinterWait(0));
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
                            yield return StartCoroutine(EndBattle());
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
            baseMenuFlavorText.fullText = agentName + " attacks!";
            yield return StartCoroutine(TextPrinterWait(0));
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
                            yield return  StartCoroutine(EndBattle());
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
        StartCoroutine(AgentScale(currentActiveAgent,new Vector3(0.6f,0.6f,.6f)));
        state = battleStateMachine.Text;
        int orderOffset = (startingAgentOrderSize - AgentOrder.ToArray().Length);

        if (turnEND)
        {
            Debug.Log("EnemyEnemy Turn" + currentActiveAgentInt.ToString());
            currentActiveAgent = EnemyArray[0];
            currentActiveAgentInt = 0;
            state = battleStateMachine.EnemyTurn;
            StartCoroutine(EnemyTurn(0));
            turnEND = false;
            return;
        }


        if (currentActiveAgentInt >= (AgentOrder.ToArray().Length-1))
        {
            StartCoroutine(ExecuteTurn());
            return;
        }


        for (int i = 0; i < PlayerCount; i++)
        {
            if (currentActiveAgent == PlayerArray[i] && currentActiveAgentInt >= EnemyCount)
            {

                if (PlayerCount > i + 1)
                {
                    Debug.Log("PlayerPlayer Turn" + currentActiveAgentInt.ToString());
                    currentActiveAgent = PlayerArray[i + 1];
                    state = battleStateMachine.PlayerTurn;
                    currentActiveAgentInt = EnemyCount + i + 1;
                    PlayerTurn();
                    return;
                }
                else
                {
                    Debug.Log("PlayerEnemy Turn" + currentActiveAgentInt.ToString());
                    currentActiveAgent = EnemyArray[0];
                    currentActiveAgentInt = 0 + orderOffset;
                    state = battleStateMachine.EnemyTurn;
                    StartCoroutine(EnemyTurn(0));
                    return;
                }

            }
        }


        for (int i = 0; i <= EnemyArray.Length; i++)
        {
            if (i == EnemyArray.Length)
            {

                currentActiveAgent = PlayerArray[0];
                currentActiveAgentInt = EnemyCount;
                state = battleStateMachine.PlayerTurn;
                Debug.Log("EnemyPlayer Turn" + currentActiveAgentInt.ToString());
                PlayerTurn();
                return;
            }
            if (EnemyArray[i].GetComponent<BattleAgent>().agentHasGone == true || EnemyArray[i].activeSelf == false)
            {
                //currentActiveAgent = EnemyArray[i];
                //currentActiveAgentInt = i + orderOffset;
                
                continue;
            }
            else
            {
                Debug.Log("EnemyEnemy Turn" + currentActiveAgentInt.ToString());
                currentActiveAgent = EnemyArray[i];
                currentActiveAgentInt = i;
                state = battleStateMachine.EnemyTurn;
                StartCoroutine(EnemyTurn(i));
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
            yield return new WaitForFixedUpdate();
            agent.transform.localScale = Vector3.Lerp(agent.transform.localScale, desiredScale, 5f * Time.deltaTime);
        }
        yield return null;

    }

    IEnumerator ExecuteTurn()
    {
        StartCoroutine(BattleOverlayAnimationScript.Animation(3, 0));
        yield return StartCoroutine(BattleOverlayAnimationScript.Animation(0, 0));
        yield return StartCoroutine(BattleOverlayAnimationScript.Animation(1, 0));
        
        RectTransform rect = AgentOrderBox.GetComponent<RectTransform>();
        int _yo = -362;
        int _yn = 0;
        for (int i = 0; i < moves.Length; i++)
        {

            if (i != 0)
            {
                _yn -= 70;
                if (activeAgentOrderBoxCoroutine != null)
                {
                    StopCoroutine(activeAgentOrderBoxCoroutine);
                }
                activeAgentOrderBoxCoroutine = StartCoroutine(PrivMath.Vector2LerpRect(rect, new Vector2(rect.localPosition.x, (_yo+_yn)), 0.05f, 100, 1));
            }
            if (moves[i] == null)
            {
                for (int j = 0; j < moves.Length; j++)
                {
                    moves[j] = null;
                    currentActiveAgentInt = 0;
                }
                CalculateSpeed();
                ChangeCurrentAgent();
                yield return null;
            }

            if (moves[i] == null)
            {
                Debug.Log("Null Move");
                continue;
            }

            if (!moves[i].attackingAgent.GetComponent<BattleAgent>().isActiveAndEnabled)
            {
                continue;
            }


            if (moves[i].move == "Attack")
            {
                battleImages[i].GetComponent<Image>().color = Color.red;
                Debug.Log("Attack Move");
                currentActiveAgent = moves[i].attackingAgent;
                targetedAgent = moves[i].targetedAgent;
                while (!targetedAgent.isActiveAndEnabled)
                {
                    if (currentActiveAgent.GetComponent<BattleAgent>().agentIdentity == true)
                    {
                        int ran = UnityEngine.Random.Range(0, BaseEnemyCount);
                        targetedAgent = EnemyArray[ran].GetComponent<BattleAgent>();
                    }
                    if (currentActiveAgent.GetComponent<BattleAgent>().agentIdentity == false)
                    {
                        int ran = UnityEngine.Random.Range(0, BasePlayerCount);
                        targetedAgent = PlayerArray[ran].GetComponent<BattleAgent>();
                    }
                }
                yield return StartCoroutine(BasicAttack(currentActiveAgent.GetComponent<BattleAgent>(), true));
                battleImages[i].GetComponent<Image>().color = Color.white;
                continue;
            }
            if (moves[i].move == "Defend")
            {
                Debug.Log("Defend Move");
                continue;
            }
            if (moves[i].move == "Heal")
            {
                battleImages[i].GetComponent<Image>().color = Color.green;
                currentActiveAgent = moves[i].attackingAgent;
                targetedAgent = moves[i].targetedAgent;

                while (!targetedAgent.isActiveAndEnabled)
                {
                    if (currentActiveAgent.GetComponent<BattleAgent>().agentIdentity == false)
                    {
                        int ran = UnityEngine.Random.Range(0, EnemyCount);
                        targetedAgent = EnemyArray[ran].GetComponent<BattleAgent>();
                    }
                    if (currentActiveAgent.GetComponent<BattleAgent>().agentIdentity == true)
                    {
                        int ran = UnityEngine.Random.Range(0, PlayerCount);
                        targetedAgent = PlayerArray[ran].GetComponent<BattleAgent>();
                    }
                }

                yield return StartCoroutine(BasicHeal(moves[i].targetedAgent));
                battleImages[i].GetComponent<Image>().color = Color.white;
                continue;
            }
        }
        for (int i = 0; i < moves.Length; i++)
        {
            moves[i] = null;
            currentActiveAgentInt = 0;
            
        }

        currentActiveAgent = PlayerArray[PlayerCount - 1];
        turnEND = true;

        StartCoroutine(BattleOverlayAnimationScript.Animation(2, 0));
        yield return StartCoroutine(BattleOverlayAnimationScript.Animation(0, 0));
        yield return StartCoroutine(BattleOverlayAnimationScript.Animation(1, 0));

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
            baseMenuFlavorText.fullText = "You have lost.";
            yield return StartCoroutine(TextPrinterWait(0));
            //give gameover screen
            yield return new WaitForSeconds(3);
            sceneLoader.SceneLoaded(0);
        }
        if (state == battleStateMachine.Flee)
        {
            baseMenuFlavorText.fullText = "You have fleed.";
            yield return StartCoroutine(TextPrinterWait(0));
            //give gameover screen
            yield return new WaitForSeconds(3);
            JSONSave.LoadFromJSON(1, 0);
        }
    }
        
}




