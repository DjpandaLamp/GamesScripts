using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataModifiableScript : MonoBehaviour
{
    private int count = 20;


    public string[] agentName; //Name of Enemy
    public int[] agentType; //Basic or Boss Enemy
    public Sprite[] agentSprite; //Sprite of the Enemy

    public int[] agentLV; //EnemyLevel, used for scaling
    public float[] agentHPMax; //Enemy Max HP
    public float[] agentHPCurrent; //Enemy Current HP
    public float[] agentENMax; //Enemy Max Energy
    public float[] agentENCurrent; //Enemy current Energy
    public float[] agentATK; //Enemy attack stat
    public float[] agentEATK;
    public float[] agentDEF; //Enemy defense stat
    public float[] agentEDEF;
    public float[] agentSPD; //Enemy speed stat
    public float[] agentEXP;


    public bool[] agentPlayerCheck;
    public int[] agentLevelPoint;


    private void Awake()
    {

        agentName = new string[count]; //Name of Enemy
        agentType = new int[count]; //Basic or Boss Enemy
                                    //  agentSprite = new Sprite[count]; //Sprite of the Enemy

        agentPlayerCheck = new bool[count];
        agentLV = new int[count];//EnemyLevel, used for scaling
        agentHPMax = new float[count]; //Enemy Max HP
        agentHPCurrent = new float[count]; //Enemy Current HP
        agentENMax = new float[count]; //Enemy Max Energy
        agentENCurrent = new float[count]; //Enemy current Energy
        agentATK = new float[count]; //Enemy attack stat
        agentEATK = new float[count];
        agentDEF = new float[count];//Enemy defense stat
        agentEDEF = new float[count];
        agentSPD = new float[count];//Enemy speed stat

        

        //Player - 1
        #region
        agentName[1] = "Whit";
        agentType[1] = 0;

        agentLV[1] = 150000;
        agentHPMax[1] = 13 + (0.1f * agentLV[1]);
        agentHPCurrent[1] = 13 + (0.1f * agentLV[1]);
        agentENMax[1] = 20 + (0.1f * agentLV[1]);
        agentENCurrent[1] = 20 + (0.1f * agentLV[1]);
        agentATK[1] = 10 + (0.1f * agentLV[1]);
        agentEATK[1] = 15 + (0.1f * agentLV[1]);
        agentDEF[1] = 4 + (0.1f * agentLV[1]);
        agentEDEF[1] = 2 + (0.1f * agentLV[1]);
        agentSPD[1] = 5 + (0.1f * agentLV[1]);
        agentPlayerCheck[1] = true;
        #endregion

        //Player - 2
        #region
        agentName[2] = "Gale";
        agentType[2] = 0;

        agentLV[2] = 140000;
        agentHPMax[2] = 12 + (0.1f * agentLV[2]);
        agentHPCurrent[2] = 12 + (0.1f * agentLV[2]);
        agentENMax[2] = 30 + (0.1f * agentLV[2]); ;
        agentENCurrent[2] = 30 + (0.1f * agentLV[2]); ;
        agentATK[2] = 8 + (0.1f * agentLV[2]); ;
        agentEATK[2] = 16 + (0.1f * agentLV[2]); ;
        agentDEF[2] = 5 + (0.1f * agentLV[2]);
        agentEDEF[2] = 7 + (0.1f * agentLV[2]);
        agentSPD[2] = 6 + (0.1f * agentLV[2]);
        agentPlayerCheck[2] = true;
        #endregion

        //Player - 3
        #region
        agentName[3] = "Gen";
        agentType[3] = 0;

        agentLV[3] = 200000;
        agentHPMax[3] = 20 + (0.1f * agentLV[3]);
        agentHPCurrent[3] = 20 + (0.1f * agentLV[3]);
        agentENMax[3] = 30 + (0.1f * agentLV[3]);
        agentENCurrent[3] = 30 + (0.1f * agentLV[3]);
        agentATK[3] = 13 + (0.1f * agentLV[3]);
        agentEATK[3] = 7 + (0.1f * agentLV[3]);
        agentDEF[3] = 9 + (0.1f * agentLV[3]);
        agentEDEF[3] = 90 + (0.1f * agentLV[3]);
        agentSPD[3] = 3 + (0.1f * agentLV[3]);
        agentPlayerCheck[3] = true;
        #endregion
    }
}
