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
    public double[] agentHPMax; //Enemy Max HP
    public double[] agentHPCurrent; //Enemy Current HP
    public double[] agentENMax; //Enemy Max Energy
    public double[] agentENCurrent; //Enemy current Energy
    public double[] agentATK; //Enemy attack stat
    public double[] agentEATK;
    public double[] agentDEF; //Enemy defense stat
    public double[] agentEDEF;
    public double[] agentSPD; //Enemy speed stat
    public double[] agentCritRate;
    public double[] agentCritDamage;
    public double[] agentEXP;



    public bool[] agentPlayerCheck;
    public int[] agentLevelPoint;


    private void Awake()
    {

        agentName = new string[count]; //Name of Enemy
        agentType = new int[count]; //Basic or Boss Enemy
                                    //  agentSprite = new Sprite[count]; //Sprite of the Enemy

        agentPlayerCheck = new bool[count];
        agentLV = new int[count];//EnemyLevel, used for scaling
        agentHPMax = new double[count]; //Enemy Max HP
        agentHPCurrent = new double[count]; //Enemy Current HP
        agentENMax = new double[count]; //Enemy Max Energy
        agentENCurrent = new double[count]; //Enemy current Energy
        agentATK = new double[count]; //Enemy attack stat
        agentEATK = new double[count];
        agentDEF = new double[count];//Enemy defense stat
        agentEDEF = new double[count];
        agentSPD = new double[count];//Enemy speed stat
        agentCritRate = new double[count];//Enemy Critical Rating stat
        agentCritDamage = new double[count];//Enemy Critical Damage stat


        //Player - 1
        #region
        agentName[1] = "Whit";
        agentType[1] = 0;

        agentLV[1] = 1550;
        agentHPMax[1] = 13 + (0.1f * agentLV[1]);
        agentHPCurrent[1] = 13 + (0.1f * agentLV[1]);
        agentENMax[1] = 20 + (0.1f * agentLV[1]);
        agentENCurrent[1] = 20 + (0.1f * agentLV[1]);
        agentATK[1] = 10 + (0.1f * agentLV[1]);
        agentEATK[1] = 15 + (0.1f * agentLV[1]);
        agentDEF[1] = 4 + (0.1f * agentLV[1]);
        agentEDEF[1] = 2 + (0.1f * agentLV[1]);
        agentSPD[1] = 5 + (0.1f * agentLV[1]);
        agentCritDamage[0] = 1.5f;
        agentCritRate[0] = 10;
        agentPlayerCheck[1] = true;
        #endregion

        //Player - 2
        #region
        agentName[2] = "Gale";
        agentType[2] = 0;

        agentLV[2] = 1410;
        agentHPMax[2] = 12 + (0.1f * agentLV[2]);
        agentHPCurrent[2] = 12 + (0.1f * agentLV[2]);
        agentENMax[2] = 30 + (0.1f * agentLV[2]); ;
        agentENCurrent[2] = 30 + (0.1f * agentLV[2]); ;
        agentATK[2] = 8 + (0.1f * agentLV[2]); ;
        agentEATK[2] = 16 + (0.1f * agentLV[2]); ;
        agentDEF[2] = 5 + (0.1f * agentLV[2]);
        agentEDEF[2] = 7 + (0.1f * agentLV[2]);
        agentSPD[2] = 6 + (0.1f * agentLV[2]);
        agentCritDamage[0] = 1.5f;
        agentCritRate[0] = 10;
        agentPlayerCheck[2] = true;
        #endregion

        //Player - 3
        #region
        agentName[3] = "Gen";
        agentType[3] = 0;

        agentLV[3] = 2179;
        agentHPMax[3] = 20 + (0.1f * agentLV[3]);
        agentHPCurrent[3] = 20 + (0.1f * agentLV[3]);
        agentENMax[3] = 30 + (0.1f * agentLV[3]);
        agentENCurrent[3] = 30 + (0.1f * agentLV[3]);
        agentATK[3] = 13 + (0.1f * agentLV[3]);
        agentEATK[3] = 7 + (0.1f * agentLV[3]);
        agentDEF[3] = 9 + (0.1f * agentLV[3]);
        agentEDEF[3] = 90 + (0.1f * agentLV[3]);
        agentSPD[3] = 3 + (0.1f * agentLV[3]);
        agentCritDamage[0] = 1.5f;
        agentCritRate[0] = 10;
        agentPlayerCheck[3] = true;
        #endregion
    }
}
