using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum battleStateMachine
{
    Start,
    PlayerTurn,
    EnemyTurn,
    Win,
    Lose
}

public class BattleSystem : MonoBehaviour
{

    public battleStateMachine state;
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    public GameObject visualCanvasElement;

    public GameObject[] EnemyArray;

    public Transform[] PlayerTransformArray;
    public Transform[] EnemyTransformArray;

    

    public int EnemyCount; //Number of Enemies to be Generated

    // Start is called before the first frame update
    void Start()
    {
        EnemyArray= new GameObject[EnemyCount];



        state = battleStateMachine.Start;
        BattleInitalize();
    }

    void BattleInitalize()
    {
        //LoadPlayerUnit();
        LoadEnemyUnit();

        state = battleStateMachine.PlayerTurn;
    }
    void LoadPlayerUnit()
    {

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

}
