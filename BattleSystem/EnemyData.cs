using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    private int count = 20;


    public string[] agentName; //Name of Enemy
    public int[] agentType; //Basic or Boss Enemy
    public Sprite[] agentSprite; //Sprite of the Enemy

    public int[] agentLV; //EnemyLevel, used for scaling
    public int[] agentHPMax; //Enemy Max HP
    public int[] agentHPCurrent; //Enemy Current HP
    public int[] agentENMax; //Enemy Max Energy
    public int[] agentENCurrent; //Enemy current Energy
    public int[] agentATK; //Enemy attack stat
    public int[] agentEATK;
    public int[] agentDEF; //Enemy defense stat
    public int[] agentEDEF;
    public int[] agentSPD; //Enemy speed stat

    public int[] agentEXPReward;
    public int agentTotalEXP;
    public int[] agentLevelPoint;


    private void Awake()
    {

        agentName = new string[count]; //Name of Enemy
        agentType = new int[count]; //Basic or Boss Enemy
                                    //  agentSprite = new Sprite[count]; //Sprite of the Enemy


        agentLV = new int[count];//EnemyLevel, used for scaling
        agentHPMax = new int[count]; //Enemy Max HP
        agentHPCurrent = new int[count]; //Enemy Current HP
        agentENMax = new int[count]; //Enemy Max Energy
        agentENCurrent = new int[count]; //Enemy current Energy
        agentATK = new int[count]; //Enemy attack stat
        agentEATK= new int[count];
        agentDEF = new int[count];//Enemy defense stat
        agentEDEF = new int[count];
        agentSPD = new int[count];//Enemy speed stat

        agentEXPReward = new int[count];



        //Testy - 0
        #region
        agentName[0] = "Testy";
        agentType[0] = 0;

        agentLV[0] = 0;
        agentHPMax[0] = 100;
        agentHPCurrent[0] = 100;
        agentENMax[0] = 100;
        agentENCurrent[0] = 100;
        agentATK[0] = 100;
        agentEATK[0] = 100;
        agentDEF[0] = 75;
        agentEDEF[0] = 75;
        agentSPD[0] = 20;
        #endregion

        //Player - 1
        #region
        agentName[1] = "Whit";
        agentType[1] = 0;

        agentLV[1] = 1;
        agentHPMax[1] = 50;
        agentHPCurrent[1] = 50;
        agentENMax[1] = 70;
        agentENCurrent[1] = 65;
        agentATK[1] = 150;
        agentEATK[1] = 150;
        agentDEF[1] = 85;
        agentEDEF[1] = 75;
        agentSPD[1] = 20;
        #endregion

        //Player - 2
        #region
        agentName[2] = "Gale";
        agentType[2] = 0;

        agentLV[2] = 1;
        agentHPMax[2] = 250;
        agentHPCurrent[2] = 250;
        agentENMax[2] = 110;
        agentENCurrent[2] = 70;
        agentATK[2] = 150;
        agentEATK[2] = 150;
        agentDEF[2] = 75;
        agentEDEF[2] = 75;
        agentSPD[2] = 20;
        #endregion

        //Player - 3
        #region
        agentName[3] = "Gen";
        agentType[3] = 0;

        agentLV[3] = 1;
        agentHPMax[3] = 400;
        agentHPCurrent[3] = 400;
        agentENMax[3] = 30;
        agentENCurrent[3] = 30;
        agentATK[3] = 120;
        agentEATK[3] = 180;
        agentDEF[3] = 90;
        agentEDEF[3] = 90;
        agentSPD[3] = 20;
        #endregion








    }

    public void CallSkill(int skillIndex)
    {
        switch(skillIndex) { 
            
            case 0: break;
        
            case 1: 
                
                return;
        }

    
    }
}
