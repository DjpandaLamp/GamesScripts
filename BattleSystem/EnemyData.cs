using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    private int count = 2;


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
        agentName[1] = "Player";
        agentType[1] = 0;

        agentLV[1] = 99;
        agentHPMax[1] = 25000;
        agentHPCurrent[1] = 25000;
        agentENMax[1] = 100;
        agentENCurrent[1] = 100;
        agentATK[1] = 150;
        agentEATK[1] = 150;
        agentDEF[1] = 75;
        agentEDEF[1] = 75;
        agentSPD[1] = 20;
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
