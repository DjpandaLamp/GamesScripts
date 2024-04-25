using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GlobalPersistantScript : MonoBehaviour
{
    public Transform playerSavedTransform;
    public EnemyData data;

    public string p1NM;
    public int p1HP;
    public int p1EN;
    public int p1AT;
    public int p1DF;
    public int p1EAT;
    public int p1EDF;
    public int p1MHP;
    public int p1MEN;
    public int p1LV;

    public string p2NM;
    public int p2HP;
    public int p2EN;
    public int p2AT;
    public int p2DF;
    public int p2EAT;
    public int p2EDF;
    public int p2MHP;
    public int p2MEN;
    public int p2LV;

    public int[] agentLevelPoint;

    public bool firstTimeLoad; //if false, load save data

    public float globalTimeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        agentLevelPoint = new int[252];

        for (int i = 0; i < 251; i++)
        {
            agentLevelPoint[i] = (5 * i) + Mathf.RoundToInt(Mathf.Pow(i, 3.2f));
        }


        p1NM = data.agentName[1];
        p1HP = data.agentHPCurrent[1];
        p1EN = data.agentENCurrent[1];
        p1AT = data.agentATK[1];
        p1DF = data.agentDEF[1];
        p1EAT = data.agentEATK[1];
        p1EDF = data.agentEDEF[1];
        p1MHP = data.agentHPMax[1];
        p1MEN = data.agentENMax[1];
        p1LV = data.agentLV[1];

        p2NM = data.agentName[2];
        p2HP = data.agentHPCurrent[2];
        p2EN = data.agentENCurrent[2];
        p2AT = data.agentATK[2];
        p2DF = data.agentDEF[2];
        p2EAT = data.agentEATK[2];
        p2EDF = data.agentEDEF[2];
        p2MHP = data.agentHPMax[2];
        p2MEN = data.agentENMax[2];
        p2LV = data.agentLV[2];


    }

    private void Update()
    {
        globalTimeElapsed += Time.deltaTime;
    }

    // Update is called once per frame
    void EXPGrowth()
    {

    }





}

