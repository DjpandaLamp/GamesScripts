using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEditor;
using UnityEngine.UI;
using Mathfunctions;


public enum battleStateMachine
{
    Start,
    PlayerTurn,
    EnemyTurn,
    Text,
    PlayerPlayerTargeting,
    PlayerEnemyTargeting,
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
    public int attackType;
    public int skillIndex;
}


public class BattleSystem : MonoBehaviour
{
    public GameObject baseMenu;
    public TypeWriter baseMenuFlavorText;
    public TypeWriter skillMenuText;
    public TextMeshProUGUI skillMenuTitle;
    public GameObject[] baseMenuButtons;
    public GameObject TextArrow;
    public GameObject skillMenu;
    public GameObject AgentOrderBox;
    public GameObject skillContainer;

    public JSONSave JSONSave;
    public battleStateMachine state;
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    public GameObject SkillButtonPrefab;
    public AudioSource audioSource;
    public EnemyData enemyData;

    public GameObject visualCanvasElement;

    public List<GameObject> PlayerArray;
    public List<GameObject> PlayerStartingArray; 
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
    private Coroutine activeTargeting;

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
    public PlayerDataModifiableScript modifiableScript;

    public bool turnEND = false;

    public SceneLoader sceneLoader;







    // Start is called before the first frame update
    void Start()
    {
        modifiableScript = GameObject.FindWithTag("Persistant").GetComponent<PlayerDataModifiableScript>();
        enemyData = GetComponent<EnemyData>();

        
        EnemyArray = new GameObject[EnemyCount];

        BasePlayerCount = PlayerCount;
        BaseEnemyCount = EnemyCount;
        moves = new MoveInfo[PlayerCount + EnemyCount];
        JSONSave = GameObject.Find("PersistantObject").GetComponent<JSONSave>();
        sceneLoader = GameObject.Find("PersistantObject").GetComponent<SceneLoader>();

        TextArrow.gameObject.SetActive(false);

        state = battleStateMachine.Start;
        StartCoroutine(BattleInitalize());

  

    }

    IEnumerator BattleInitalize()
    {
        LoadPlayerUnit();
        yield return StartCoroutine(LoadEnemyUnit());


        currentActiveAgent = EnemyArray[0];
        yield return new WaitForEndOfFrame();
        CalculateSpeed();
        PopulateSkills();
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
            PlayerStartingArray.Add(NewPlayer);
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
        yield return new WaitForSeconds(0.01f);
        EnemyArray[enemyIndex].GetComponent<BattleAgent>().agentHasGone = true;
        if (EnemyArray[enemyIndex].activeSelf == true)
        {
            
            //Enemy Basic Attack
            #region
            string agentName = "";
            agentName = EnemyArray[enemyIndex].GetComponent<BattleAgent>().agentName;
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
            move.attackType = 0;
            moves[EnemyArray[enemyIndex].GetComponent<BattleAgent>().currentBattleSpeedIndex] = move;
            ChangeCurrentAgent();
        
            #endregion
        }
        else
        {
            ChangeCurrentAgent();
        }
    }

    public void CallSkillPublic(int index)
    {
        StartCoroutine(CallSkill(index));
    }
    IEnumerator CallSkill(int index)
    {
        SpellData spell = enemyData.CallSkill(index);


        
        MoveInfo move = movesContainer.AddComponent<MoveInfo>();
        Debug.Log(index);
        Debug.Log(spell.spellName);


        if (spell.isDamageorHeal == true)
        {
            state = battleStateMachine.PlayerPlayerTargeting;
            move.move = "Heal";
            while (targetedAgent == null)
            {

                if (isBackPressed)
                {
                    isBackPressed = false;
                    state = battleStateMachine.PlayerTurn;
                    PlayerTurn();
                    targetedAgent = null;
                    audioSource.Play();
                    PlayerArray[playerCursorPos].GetComponent<BattleAgent>().agentHealthFillRect.color = PlayerArray[playerCursorPos].GetComponent<BattleAgent>().agentHealthSliderBaseColor;
                    StopCoroutine(activeTargeting);
                    activeTargeting = null;
                    yield return null;
                }
                activeTargeting = StartCoroutine(PlayerPlayerTargeting());
                yield return activeTargeting;
            }
        }
        else
        {
            state = battleStateMachine.PlayerEnemyTargeting;
            move.move = "Attack";
            while (targetedAgent == null)
            {
                if (isBackPressed)
                {
                    isBackPressed = false;
                    state = battleStateMachine.PlayerTurn;
                    PlayerTurn();
                    targetedAgent = null;
                    audioSource.Play();
                    yield return null;
                }
                yield return StartCoroutine(PlayerEnemyTargeting());
            }
        }
        if (targetedAgent != null)
        {
            move.attackingAgent = currentActiveAgent;
            move.targetedAgent = targetedAgent;
            move.attackType = 0;
            move.skillIndex = index;
            moves[currentActiveAgent.GetComponent<BattleAgent>().currentBattleSpeedIndex] = move;
            ChangeCurrentAgent();
        }




    }

    void PopulateSkills()
    {
        for (int i = 0; i < 5; i++)
        {
            SkillPopulateScript skillScript = Instantiate(SkillButtonPrefab, skillContainer.transform).GetComponent<SkillPopulateScript>();
            skillScript.spellIndex = i;
            skillScript.spellText.text = enemyData.spells[i].spellName;
            skillScript.costText.text = "Cost: " + enemyData.spells[i].ENCost.ToString();
            skillScript.powerText.text = "Power: " +(enemyData.spells[i].dmgRatio*100).ToString();
            if (enemyData.spells[i].isDamageorHeal)
            {
                skillScript.typeText.text = "Heal";
            }
            else
            {
                skillScript.typeText.text = "Attack";
            }
            skillScript.system = GetComponent<BattleSystem>();
            skillScript.setButton();
            

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
       
       // AudioClip clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/SFX/MenuValidInput2.wav", typeof(AudioClip));
       // audioSource.clip = audioClip.Result;
       // audioSource.Play();
        Debug.Log("AttackButtonPressed");
        if (state != battleStateMachine.PlayerTurn && state != battleStateMachine.PlayerEnemyTargeting)
        {
            yield return 0;
        }

        state = battleStateMachine.PlayerEnemyTargeting;
        baseMenuFlavorText.fullText = "Pick your Target.";
        yield return StartCoroutine(TextPrinterWait(1));
        while (targetedAgent == null)
        {
            if (isBackPressed || state != battleStateMachine.PlayerEnemyTargeting)
            {
                isBackPressed = false;
                state = battleStateMachine.PlayerTurn;
                PlayerTurn();
                audioSource.Play();
                playerCursorPos = 0;
                targetedAgent = null;
                yield break;
            }

                yield return StartCoroutine(PlayerEnemyTargeting());
            }

            if (targetedAgent == null)
            { 
            yield return null;
            }
            else
            {
                MoveInfo move = movesContainer.AddComponent<MoveInfo>();
                move.move = "Attack";
                move.attackingAgent = currentActiveAgent;
                move.targetedAgent = targetedAgent;
                move.attackType = 0;
                moves[currentActiveAgent.GetComponent<BattleAgent>().currentBattleSpeedIndex] = move;
            ChangeCurrentAgent();
        }
    }

    public void OnFleeButton()
    {



        if (state != battleStateMachine.PlayerTurn)
        {
            return;
        }
       
        //AudioClip clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/SFX/MenuValidInput2.wav", typeof(AudioClip));
        //audioSource.clip = audioClip.Result;
       // audioSource.Play();
        state = battleStateMachine.Flee;
        StartCoroutine(EndBattle());
    }

    public void OnSkillButton()
    {
        
       // AudioClip clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/SFX/MenuValidInput2.wav", typeof(AudioClip));
       // audioSource.clip = audioClip.Result;
       // audioSource.Play();
        state = battleStateMachine.PlayerSkill;
    }

   

    public void OnBackButton()
    {
        isBackPressed = true;
    }

    public void OnDefenseButton()
    {
        MoveInfo move = movesContainer.AddComponent<MoveInfo>();
        move.move = "Defend";
        move.attackingAgent = currentActiveAgent;
        move.targetedAgent = move.attackingAgent.GetComponent<BattleAgent>();
        moves[currentActiveAgent.GetComponent<BattleAgent>().currentBattleSpeedIndex] = move;
        ChangeCurrentAgent();
    }



    public IEnumerator OnDefenseEnumberable()
    {
        BattleAgent agent = currentActiveAgent.GetComponent<BattleAgent>();
        agent.isDefending = true;
        baseMenuFlavorText.fullText = agent.agentName + " Defended.";
        yield return StartCoroutine(TextPrinterWait(0));
        ChangeCurrentAgent();
    }

    IEnumerator PlayerEnemyTargeting()
    {
        if (state != battleStateMachine.PlayerEnemyTargeting)
        {
            yield break;
        }
        if (isBackPressed)
        {
            yield break;
        }
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
        targetColorBlend = Mathf.Clamp01(0.6f + Mathf.Sin(Time.time) / 2);
        Color baseColor = EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentHealthSliderBaseColor;
        Color blend = new Color(1, 0, 1, 1);
        Color blendColor = (Color.Lerp(baseColor, blend, 0.5f + Mathf.PingPong(Time.time, 0.5f)));
        EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentHealthFillRect.color = blendColor;

        

        if (Input.GetKeyDown("z") && EnemyArray[playerCursorPos].activeSelf == true)
        {
            targetedAgent = EnemyArray[playerCursorPos].GetComponent<BattleAgent>();
        }

        if (targetedAgent == null)
        {
            yield return null;
        }

        EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentHealthFillRect.color = EnemyArray[playerCursorPos].GetComponent<BattleAgent>().agentHealthSliderBaseColor;
        yield break;
    }

    IEnumerator PlayerPlayerTargeting()
    {
        if (state != battleStateMachine.PlayerPlayerTargeting)
        {
            yield break;
        }
        if (isBackPressed)
        {
            yield break;
        }

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

        if (PlayerArray.ToArray().Length - 1 < playerCursorPos)
        {
            playerCursorPos = 0;
        }
        if (playerCursorPos < 0)
        {
            playerCursorPos = PlayerArray.ToArray().Length - 1;
        }

        while (PlayerArray[playerCursorPos].activeSelf == false)
        {
            if (dir == true)
            {
                playerCursorPos -= 1;
            }
            else
            {
                playerCursorPos += 1;
            }
            if (PlayerArray.ToArray().Length - 1 < playerCursorPos)
            {
                playerCursorPos = 0;
            }
            if (playerCursorPos < 0)
            {
                playerCursorPos = PlayerArray.ToArray().Length - 1;
            }

        }

        targetColorBlend = Mathf.Clamp01(0.6f + Mathf.Sin(Time.time) / 2);
        Color baseColor = PlayerArray[playerCursorPos].GetComponent<BattleAgent>().agentHealthSliderBaseColor;
        Color blend = new Color(1, 0, 1, 1);
        Color blendColor = (Color.Lerp(baseColor, blend, 0.5f + Mathf.PingPong(Time.time, 0.5f)));
        PlayerArray[playerCursorPos].GetComponent<BattleAgent>().agentHealthFillRect.color = blendColor;


        if (Input.GetKeyDown("z")  && PlayerArray[playerCursorPos].activeSelf == true)
        {
            targetedAgent = PlayerArray[playerCursorPos].GetComponent<BattleAgent>();
        }

        if (targetedAgent == null)
        {
            yield return null;
        }
        PlayerArray[playerCursorPos].GetComponent<BattleAgent>().agentHealthFillRect.color = PlayerArray[playerCursorPos].GetComponent<BattleAgent>().agentHealthSliderBaseColor;
        yield break;
    }


    

    IEnumerator BasicAttack(BattleAgent attackingAgent, bool attackDirection, int attackType)
    {
        string agentName = attackingAgent.agentName;

        //attackDirection = true means player is attacking
        //attackDirection = false means Enemy is attacking



        Debug.Log("AgentTargeted: " + targetedAgent.agentName);

        double agentPreDamageHealth = targetedAgent.agentHPCurrent;

        Vector2 isDefeated = targetedAgent.TakeDamage(attackingAgent.agentATKFull, (float)attackingAgent.agentCritRate, (float)attackingAgent.agentCritDamage, attackType);

        //Player Attack
        if (attackDirection == true)
        {

            state = battleStateMachine.Text;
            baseMenuFlavorText.fullText = agentName + " attacks!";
            yield return StartCoroutine(TextPrinterWait(0));
            switch(isDefeated.y)
            {
                case 0:
                    break;
                case 1:
                    baseMenuFlavorText.fullText = "The attack was Critical (x" + Math.Pow(attackingAgent.agentCritDamage,isDefeated.y).ToString() + ")";
                    yield return StartCoroutine(TextPrinterWait(0));
                    break;
                case 2:
                    baseMenuFlavorText.fullText = "The attack was Supercritical (x" + Math.Pow(attackingAgent.agentCritDamage,isDefeated.y).ToString() + ")";
                    yield return StartCoroutine(TextPrinterWait(0));
                    break;
                case 3:
                    baseMenuFlavorText.fullText = "The attack was Megacritical(x" + Math.Pow(attackingAgent.agentCritDamage,isDefeated.y).ToString() + ")";;
                    yield return StartCoroutine(TextPrinterWait(0));
                    break;
                case 4:
                    baseMenuFlavorText.fullText = "The attack was Ultracritical (x" + Math.Pow(attackingAgent.agentCritDamage,isDefeated.y).ToString() + ")";;
                    yield return StartCoroutine(TextPrinterWait(0));
                    break;
                case 5:
                    baseMenuFlavorText.fullText = "The attack was Gigacritical (x" + Math.Pow(attackingAgent.agentCritDamage,isDefeated.y).ToString() + ")";;
                    yield return StartCoroutine(TextPrinterWait(0));
                    break;
                case 6:
                    baseMenuFlavorText.fullText = "The attack was Hypercritical (x" + Math.Pow(attackingAgent.agentCritDamage, isDefeated.y).ToString() + ")";
                    yield return StartCoroutine(TextPrinterWait(0));
                    break;
                case 7:
                    baseMenuFlavorText.fullText = "The attack was Teracritical (x" + Math.Pow(attackingAgent.agentCritDamage, isDefeated.y).ToString() + ")";
                    yield return StartCoroutine(TextPrinterWait(0));
                    break;
                case 8:
                    baseMenuFlavorText.fullText = "The attack was a Demi-Critical (x" + Math.Pow(attackingAgent.agentCritDamage, isDefeated.y).ToString() + ")"; ;
                    yield return StartCoroutine(TextPrinterWait(0));
                    break;
                case 9:
                    baseMenuFlavorText.fullText = "The attack was a Demonic Critical (x" + Math.Pow(attackingAgent.agentCritDamage, isDefeated.y).ToString() + ")"; ;
                    yield return StartCoroutine(TextPrinterWait(0));
                    break;
                case 10:
                    baseMenuFlavorText.fullText = "The attack was a Divine Critical (x" + Math.Pow(attackingAgent.agentCritDamage, isDefeated.y).ToString() + ")"; ;
                    yield return StartCoroutine(TextPrinterWait(0));
                    break;
                default:
                    baseMenuFlavorText.fullText = "The attack was Beyond Critical (x" + Math.Pow(attackingAgent.agentCritDamage, isDefeated.y).ToString() + ") (Crit Level: " + isDefeated.y.ToString() + ")" ;
                    yield return StartCoroutine(TextPrinterWait(0));
                    break;
            }

            if (isDefeated.x == 1)
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
            if (isDefeated.x == 1)
            {
                AgentOrder.Remove(PlayerArray[targetedAgentNumber].GetComponent<BattleAgent>());
                
                removedAgentCount += 1;
                PlayerArray.RemoveAt(targetedAgentNumber);
                PlayerCount = PlayerArray.ToArray().Length;
                Debug.Log(PlayerCount);
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


    IEnumerator Heal(BattleAgent attackingAgent, SpellData spell)
    {
        state = battleStateMachine.Text;

        if (attackingAgent.agentENCurrent >= spell.ENCost)
        {
            double agentPreDamageHealth = targetedAgent.agentHPCurrent;
            targetedAgent.ReceiveHeal(attackingAgent.agentEATKFull * spell.dmgRatio);
            if (agentPreDamageHealth - targetedAgent.agentHPCurrent != 0)
            {
                attackingAgent.agentENCurrent -= spell.ENCost;
            }
     

            if (attackingAgent == targetedAgent)
            {
                baseMenuFlavorText.fullText = attackingAgent.agentName + " used " + spell.spellName + ".";
                yield return StartCoroutine(TextPrinterWait(0));
            }
            else
            {
                baseMenuFlavorText.fullText = attackingAgent.agentName + " used " + spell.spellName + " on " + targetedAgent.agentName + ".";
                yield return StartCoroutine(TextPrinterWait(0));
            }

            if (agentPreDamageHealth - targetedAgent.agentHPCurrent == 0)
            {
                baseMenuFlavorText.fullText = "But the healing had no effect!";
                yield return StartCoroutine(TextPrinterWait(0));
            }
            else
            {
                state = battleStateMachine.Text;
                baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " is healed " + Math.Abs(agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " Health!";
                yield return StartCoroutine(TextPrinterWait(0));
                
            }



            yield return StartCoroutine(TextPrinterWait(0));
        }
        else
        {
            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " Didn't have enough Energy to use " + spell.spellName +  "!";
            yield return StartCoroutine(TextPrinterWait(0));
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
        if (state == battleStateMachine.PlayerEnemyTargeting || state == battleStateMachine.PlayerPlayerTargeting)
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
        if (state != battleStateMachine.PlayerTurn && state != battleStateMachine.PlayerEnemyTargeting && state != battleStateMachine.PlayerPlayerTargeting && state != battleStateMachine.PlayerSkill)
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
        int orderOffset = (startingAgentOrderSize - AgentOrder.ToArray().Length);

        if (turnEND == true)
        {

                
            currentActiveAgent = EnemyArray[0];
                currentActiveAgentInt = 0;
                state = battleStateMachine.EnemyTurn;
            StartCoroutine(EnemyTurn(0));
                turnEND = false;
                return;
            
        }
        bool isMovesFull = true;
        for (int m = 0; m < moves.Length; m++)
        {
            if (moves[m] == null)
            {
                isMovesFull = false;
            }
        }

        if (currentActiveAgentInt >= (AgentOrder.ToArray().Length-1) && isMovesFull)
        {
            StartCoroutine(ExecuteTurn());
            return;
        }


        for (int i = 0; i < PlayerCount; i++)
        {
            if (currentActiveAgent == PlayerArray[i] && currentActiveAgentInt >= EnemyCount)
            {

                if (PlayerArray[i].activeSelf == false)
                {
                    //currentActiveAgent = EnemyArray[i];
                    //currentActiveAgentInt = i + orderOffset;

                    continue;
                }
                if (PlayerCount > i + 1)
                {  
                    currentActiveAgent = PlayerArray[i + 1];
                    state = battleStateMachine.PlayerTurn;
                    currentActiveAgentInt = EnemyCount + i + 1;
                    PlayerTurn();
                    return;
                }
                else
                {
                    currentActiveAgent = EnemyArray[0];
                    currentActiveAgentInt = 0;
                    state = battleStateMachine.EnemyTurn;
                    StartCoroutine(EnemyTurn(0));
                    return;
                }

            }
        }

        int eliminatedEnemies = 0;
        for (int i = 0; i <= EnemyArray.Length; i++)
        {
            if (i == EnemyArray.Length)
            {
                int index = 0;
                currentActiveAgent = PlayerArray[index];
                currentActiveAgentInt = EnemyCount;
                state = battleStateMachine.PlayerTurn;
                PlayerTurn();
                return;
            }
            if (EnemyArray[i].GetComponent<BattleAgent>().agentHasGone == true || EnemyArray[i].activeSelf == false)
            {
                if (i == EnemyArray.Length - 1)
                {
                    int index = 0;
                    currentActiveAgent = PlayerArray[index];
                    currentActiveAgentInt = EnemyCount;
                    state = battleStateMachine.PlayerTurn;
                    PlayerTurn();
                    return;
                }
                //currentActiveAgent = EnemyArray[i];
                //currentActiveAgentInt = i + orderOffset;
                if (EnemyArray[i].activeSelf == false)
                {
                    eliminatedEnemies += 1;
                }
                
                continue;
            }
            else
            {
                currentActiveAgent = EnemyArray[i];
                currentActiveAgentInt = i-eliminatedEnemies;
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
        
       /* while (Mathf.Abs(agent.transform.localScale.x - desiredScale.x) > 0.01f)
        {
            yield return new WaitForFixedUpdate();
            agent.transform.localScale = Vector3.Lerp(agent.transform.localScale, desiredScale, 5f * Time.deltaTime);
        }*/
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
                while (!targetedAgent.isActiveAndEnabled && currentActiveAgent.GetComponent<BattleAgent>().agentIdentity == true)
                {
                    int ran = UnityEngine.Random.Range(0, BaseEnemyCount);
                    targetedAgent = EnemyArray[ran].GetComponent<BattleAgent>();
                }
                while (!targetedAgent.isActiveAndEnabled && currentActiveAgent.GetComponent<BattleAgent>().agentIdentity == false)
                {
                    int ran = UnityEngine.Random.Range(0, PlayerArray.ToArray().Length);
                    targetedAgent = PlayerArray[ran].GetComponent<BattleAgent>();
                }

                if (currentActiveAgent.GetComponent<BattleAgent>().agentIdentity == true)
                {
                    yield return StartCoroutine(BasicAttack(currentActiveAgent.GetComponent<BattleAgent>(), true, moves[i].attackType));
                }
                if (currentActiveAgent.GetComponent<BattleAgent>().agentIdentity == false)
                {
                    yield return StartCoroutine(BasicAttack(currentActiveAgent.GetComponent<BattleAgent>(), false, moves[i].attackType));
                }
                
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
                SpellData spell = enemyData.spells[moves[i].skillIndex];
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

                yield return StartCoroutine(Heal(moves[i].targetedAgent,spell));
                battleImages[i].GetComponent<Image>().color = Color.white;
                continue;
            }
        }
        for (int i = 0; i < moves.Length; i++)
        {
            moves[i] = null;
            currentActiveAgentInt = 0;
            
        }
        for (int i = 0; i < BaseEnemyCount; i++)
        {
            EnemyArray[i].GetComponent<BattleAgent>().agentHasGone = false;
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
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(GrantLV());
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

    IEnumerator GrantLV()
    {
        int enemyLVTotal = 0;
        for (int i = 0; i < EnemyArray.Length; i++)
        {
            enemyLVTotal += EnemyArray[i].GetComponent<BattleAgent>().agentLV;
        }
        yield return new WaitForSeconds(0.5f);
        JSONSave.LoadFromJSON(1, 0);
    }
        
}




