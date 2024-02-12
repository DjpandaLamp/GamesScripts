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
    public TextMeshProUGUI baseMenuFlavorText;
    public GameObject[] baseMenuButtons;


    public battleStateMachine state;
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    public GameObject visualCanvasElement;

    public GameObject[] PlayerArray;
    public GameObject[] EnemyArray;
    public GameObject[] PlayerCurrentArray;
    public GameObject[] EnemyCurrentArray;
    public bool[] PlayerSpliceArray;
    public bool[] EnemySpliceArray;

    public Transform[] PlayerTransformArray;
    public Transform[] EnemyTransformArray;

    public GameObject currentActiveAgent;
    public BattleAgent targetedAgent;

    
    public int PlayerCount;
    public int EnemyCount; //Number of Enemies to be Generated

    // Start is called before the first frame update
    void Start()
    {
        PlayerArray = new GameObject[PlayerCount];
        EnemyArray= new GameObject[EnemyCount];

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
        for (int i = 0; i<EnemyCount;i++)
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
        baseMenuFlavorText.text = "What's Your Move?";
    }

    public void OnAttackButton()
    {
        if (state != battleStateMachine.PlayerTurn)
        {
            return;
        }
        StartCoroutine(BasicAttack(PlayerArray[0].GetComponent<BattleAgent>(),true));
    }



    IEnumerator BasicAttack(BattleAgent attackingAgent, bool attackDirection)
    {
        //attackDirection = true means player is attacking
        //attackDirection = false means Enemy is attacking


        //Temp code, replace with enemy picker in near future//
        if (attackDirection == true)
        {
            targetedAgent = EnemyArray[Random.Range(0, EnemyCount)].GetComponent<BattleAgent>();
            while (targetedAgent.gameObject.activeSelf == false) //ensures that the targeted enemy is not defeated
            {
                targetedAgent = EnemyArray[Random.Range(0, EnemyCount)].GetComponent<BattleAgent>();
            }
        }

        //Temp code, replace with enemy picker in near future//


        //BattleAgent attackingAgent = PlayerArray[0].GetComponent<BattleAgent>();

        int agentPreDamageHealth = targetedAgent.agentHPCurrent;

        bool isDefeated = targetedAgent.TakeDamage(attackingAgent.agentATK);

        //Player Attack
        if (attackDirection == true)
        {
            state = battleStateMachine.Text;
            yield return new WaitForSeconds(2);
            if (isDefeated)
            {
                targetedAgent.gameObject.SetActive(false);
                for (int i = 0; i < EnemyCount; i++)
                {
                    
                    if (EnemyArray[i].activeSelf == true)
                    {
                        
                        
                    }
                    else
                    {
                        EnemyCount -= 1;
                        Debug.Log(EnemyCount.ToString());

                    }
                    if (EnemyCount == 0)
                    {
                        baseMenuFlavorText.text = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                        yield return new WaitForSeconds(2);
                        state = battleStateMachine.Win;
                        StartCoroutine(EndBattle());
                    }
                    else
                    {
                        baseMenuFlavorText.text = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                        yield return new WaitForSeconds(2);
                        baseMenuFlavorText.text = targetedAgent.agentName.ToString() + " was Defeated.";
                        yield return new WaitForSeconds(1);
                        ChangeCurrentAgent();
                    }

                }
            }
            else
            {
                
                baseMenuFlavorText.text = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                yield return new WaitForSeconds(2);
                ChangeCurrentAgent();
            }
        }
        //Enemy Attack
        if (attackDirection == false)
        {
            state = battleStateMachine.Text;
            yield return new WaitForSeconds(2);
            if (isDefeated)
            {
                targetedAgent.gameObject.SetActive(false);
                for (int i = 0; i < PlayerCount; i++)
                {
                    
                    if (PlayerArray[i].activeSelf == true)
                    {
                        
                        
                    }
                    else
                    {
                        PlayerCount -= 1;
                    }
                    if (PlayerCount == 0)
                    {
                        baseMenuFlavorText.text = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                        yield return new WaitForSeconds(2);
                        state = battleStateMachine.Lose;
                        StartCoroutine(EndBattle());
                    }
                    else
                    {
                        baseMenuFlavorText.text = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                        yield return new WaitForSeconds(2);
                        baseMenuFlavorText.text = targetedAgent.agentName.ToString() + " was Defeated.";
                        yield return new WaitForSeconds(1);
                        ChangeCurrentAgent();
                    }
                }
            }
            else
            {
                
                baseMenuFlavorText.text = targetedAgent.agentName.ToString() + " took " + (agentPreDamageHealth - targetedAgent.agentHPCurrent).ToString() + " damage!";
                yield return new WaitForSeconds(2);
                ChangeCurrentAgent();
            }
        }

        //continue to next agent turn or play death animation(s)
    }

    IEnumerator EnemyTurn(int enemyIndex)
    {
        //Enemy Basic Attack
        #region
        baseMenuFlavorText.text = EnemyArray[enemyIndex].name.ToString() + "attacks!";

        yield return new WaitForSeconds(1);

        targetedAgent = PlayerArray[0].GetComponent<BattleAgent>();
        StartCoroutine(BasicAttack(EnemyArray[enemyIndex].GetComponent<BattleAgent>(), false));
        #endregion 
    }

    private void Update()
    {
        if (state == battleStateMachine.PlayerTurn)
        {
            for (int i = 0; i<baseMenuButtons.Length;i++)
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
        Debug.Log("0");
        for (int i = 0; i < PlayerCount;i++)
        {
            if (currentActiveAgent == PlayerArray[i] )
            {
                Debug.Log("1");
                if (PlayerArray.Length > i+1)
                {
                    currentActiveAgent = PlayerArray[i+1];
                    state = battleStateMachine.PlayerTurn;
                    PlayerTurn();
                    Debug.Log("1a");
                    break;
                    
                }
                else
                {
                    currentActiveAgent = EnemyArray[0];
                    state = battleStateMachine.EnemyTurn;
                    StartCoroutine(EnemyTurn(0));
                    Debug.Log("1b");
                    break;
                    
                }

            }
            for (int j = 0; j < EnemyCount;j++)
            {
                if (currentActiveAgent == EnemyArray[j])
                {
                    Debug.Log("2");
                    if (EnemyArray.Length > j + 1) 
                    {
                        currentActiveAgent = EnemyArray[j + 1];
                        state = battleStateMachine.EnemyTurn;
                        StartCoroutine(EnemyTurn(j + 1));
                        Debug.Log("2a");
                        break;
                        
                    }
                    else
                    {
                        currentActiveAgent = PlayerArray[0];
                        state = battleStateMachine.PlayerTurn;
                        PlayerTurn();
                        Debug.Log("2b");
                        break;
                        
                    }

                }
            }
        }
        
    }


    IEnumerator EndBattle()
    {
        if (state == battleStateMachine.Win)
        {
            baseMenuFlavorText.text = "YOU WIN!!!!";
            //return to overworld
            yield return new WaitForSeconds(3);
            SceneManager.LoadSceneAsync(0);
        }
        if (state == battleStateMachine.Lose)
        {
            baseMenuFlavorText.text = "You have lost. Regret your actions and try again";
            //give gameover screen
            yield return new WaitForSeconds(3);
            SceneManager.LoadSceneAsync(0);
        }
    }



}
