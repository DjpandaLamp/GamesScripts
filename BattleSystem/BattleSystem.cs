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
    Win,
    Lose
}

public class BattleSystem : MonoBehaviour
{
    public GameObject baseMenu;
    public TypeWriter baseMenuFlavorText;
    public GameObject[] baseMenuButtons;
    public GameObject TextArrow;


    public battleStateMachine state;
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    public GameObject visualCanvasElement;

    public GameObject[] PlayerArray;
    public GameObject[] EnemyArray;
    public GameObject[] PlayerCurrentArray;
    public GameObject[] EnemyCurrentArray;


    public Transform[] PlayerTransformArray;
    public Transform[] EnemyTransformArray;

    public GameObject currentActiveAgent;
    public BattleAgent targetedAgent;

    public int BasePlayerCount;
    public int BaseEnemyCount;
    public int PlayerCount;
    public int EnemyCount; //Number of Enemies to be Generated


    // Start is called before the first frame update
    void Start()
    {

        PlayerArray = new GameObject[PlayerCount];
        EnemyArray = new GameObject[EnemyCount];

        BasePlayerCount = PlayerCount;
        BaseEnemyCount = EnemyCount;

        TextArrow.gameObject.SetActive(false);

        state = battleStateMachine.Start;
        StartCoroutine(BattleInitalize());
    }

    IEnumerator BattleInitalize()
    {
        LoadPlayerUnit();
        LoadEnemyUnit();

        currentActiveAgent = PlayerArray[0];

        yield return new WaitForSeconds(5);

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
        targetedAgent = null;
        baseMenuFlavorText.fullText = "What's Your Move?";
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
            targetedAgent = EnemyArray[Random.Range(0, BaseEnemyCount)].GetComponent<BattleAgent>();
            //targetedAgent = EnemyArray[1].GetComponent<BattleAgent>();
            while (targetedAgent.gameObject.activeSelf == false) //ensures that the targeted enemy is not defeated
            {
                targetedAgent = EnemyArray[Random.Range(0, BaseEnemyCount)].GetComponent<BattleAgent>();
            }
        }


        Debug.Log("AgentTargeted: " + targetedAgent.agentName);

        //Temp code, replace with enemy picker in near future//


        //BattleAgent attackingAgent = PlayerArray[0].GetComponent<BattleAgent>();

        int agentPreDamageHealth = targetedAgent.agentHPCurrent;

        bool isDefeated = targetedAgent.TakeDamage(attackingAgent.agentATK);

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
                            yield return StartCoroutine(TextPrinterWait());
                            targetedAgent.gameObject.SetActive(false);
                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " was Defeated.";
                            yield return StartCoroutine(TextPrinterWait());
                            state = battleStateMachine.Win;
                            StartCoroutine(EndBattle());
                        }
                        else
                        {
                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                            yield return StartCoroutine(TextPrinterWait());
                            targetedAgent.gameObject.SetActive(false);
                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " was Defeated.";
                            yield return StartCoroutine(TextPrinterWait());
                            ChangeCurrentAgent();
                        }
                    }
                }
            }
            else
            {

                baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                yield return StartCoroutine(TextPrinterWait());
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
                for (int i = 0; i < PlayerCount; i++)
                {

                    if (PlayerArray[i].activeSelf == true)
                    {


                    }
                    else
                    {
                        PlayerCount -= 1;
                    }
                    if (i == PlayerArray.Length - 1)
                    {
                        if (PlayerCount == 0)
                        {

                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                            yield return StartCoroutine(TextPrinterWait());
                            targetedAgent.gameObject.SetActive(false);
                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " was Defeated.";
                            yield return StartCoroutine(TextPrinterWait());
                            state = battleStateMachine.Lose;
                            StartCoroutine(EndBattle());
                        }
                        else
                        {
                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";

                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                            yield return StartCoroutine(TextPrinterWait());
                            targetedAgent.gameObject.SetActive(false);
                            baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " was Defeated.";
                            yield return StartCoroutine(TextPrinterWait());
                            ChangeCurrentAgent();
                        }
                    }
                }
            }
            else
            {

                baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                yield return StartCoroutine(TextPrinterWait());

                ChangeCurrentAgent();
            }
        }

        //continue to next agent turn or play death animation(s)
    }


    IEnumerator BasicHeal(BattleAgent attackingAgent, bool attackDirection)
    {
        int agentPreDamageHealth = targetedAgent.agentHPCurrent;

        targetedAgent.ReceiveHeal();

        yield return new WaitForSeconds(2);

        baseMenuFlavorText.fullText = targetedAgent.agentName.ToString() + " is healed " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " Health!";
        yield return StartCoroutine(TextPrinterWait());
        ChangeCurrentAgent();
    }


    IEnumerator EnemyTurn(int enemyIndex)
    {
        
        EnemyArray[enemyIndex].GetComponent<BattleAgent>().agentHasGone = true;
        if (EnemyArray[enemyIndex].activeSelf == true)
        {

            //Enemy Basic Attack
            #region
            string agentName = "";

            agentName = EnemyArray[enemyIndex].GetComponent<BattleAgent>().agentName;
            baseMenuFlavorText.fullText = agentName + " attacks!";
            yield return StartCoroutine(TextPrinterWait());
            
            targetedAgent = PlayerArray[0].GetComponent<BattleAgent>();
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
        if (state == battleStateMachine.PlayerTurn)
        {
            for (int i = 0; i < baseMenuButtons.Length; i++)
            {
                baseMenuButtons[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < baseMenuButtons.Length; i++)
            {
                baseMenuButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void ChangeCurrentAgent()
    {
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
                    Debug.Log("boing go");
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

    IEnumerator TextPrinterWait()
    {
        baseMenuFlavorText.isFinished = true;
        yield return new WaitForSeconds(0.01f);
        while (baseMenuFlavorText.isFinished == false)
        {
            yield return null;
        }
        while (Input.GetMouseButtonDown(0) != true)
        {
            TextArrow.gameObject.SetActive(true);
            yield return null;
        }
        TextArrow.gameObject.SetActive(false);
    }

    IEnumerator EndBattle()
    {
        if (state == battleStateMachine.Win)
        {
            baseMenuFlavorText.fullText = "YOU WIN!!!!";
            //return to overworld
            yield return new WaitForSeconds(3);
            SceneManager.LoadSceneAsync(0);
        }
        if (state == battleStateMachine.Lose)
        {
            baseMenuFlavorText.fullText = "You have lost. Regret your actions and try again";
            //give gameover screen
            yield return new WaitForSeconds(3);
            SceneManager.LoadSceneAsync(0);
        }
    }
        
}




