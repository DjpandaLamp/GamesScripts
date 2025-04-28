using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    private int count = 20;

    public PlayerDataModifiableScript modifiableScript;

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

    public int[] agentEXP;
    

    public bool[] agentPlayerCheck;
    public int[] agentLevelPoint;


    private void OnEnable()
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

        while (modifiableScript == null)
        {
            modifiableScript = GameObject.FindObjectOfType<PlayerDataModifiableScript>();
        }


        //Testy - 0
        #region
        agentName[0] = "Testy";
        agentType[0] = 0;

        agentLV[0] = 0;
        agentHPMax[0] = 100;
        agentHPCurrent[0] = 100;
        agentENMax[0] = 100;
        agentENCurrent[0] = 100;
        agentATK[0] = 200;
        agentEATK[0] = 100;
        agentDEF[0] = 75;
        agentEDEF[0] = 75;
        agentSPD[0] = 50;
        agentCritDamage[0] = 1.5f;
        agentCritRate[0] = 20;
        agentPlayerCheck[0] = false;
        #endregion

        //Player - 1
        #region
        agentName[1] = "Whit";
        agentType[1] = 0;

        agentLV[1] = modifiableScript.agentLV[1];
        agentHPMax[1] = modifiableScript.agentHPMax[1];
        agentHPCurrent[1] = modifiableScript.agentHPCurrent[1];
        agentENMax[1] = modifiableScript.agentENMax[1];
        agentENCurrent[1] = modifiableScript.agentENCurrent[1];
        agentATK[1] = modifiableScript.agentATK[1];
        agentEATK[1] = modifiableScript.agentEATK[1];
        agentDEF[1] = modifiableScript.agentDEF[1];
        agentEDEF[1] = modifiableScript.agentEDEF[1];
        agentSPD[1] = modifiableScript.agentSPD[1];
        agentCritDamage[1] = modifiableScript.agentCritDamage[1];
        agentCritRate[1] = modifiableScript.agentCritRate[1];
        agentPlayerCheck[1] = true;
        #endregion

        //Player - 2
        #region
        agentName[2] = "Gale";
        agentType[2] = 0;

        agentLV[2] = modifiableScript.agentLV[2];
        agentHPMax[2] = modifiableScript.agentHPMax[2];
        agentHPCurrent[2] = modifiableScript.agentHPCurrent[2];
        agentENMax[2] = modifiableScript.agentENMax[2];
        agentENCurrent[2] = modifiableScript.agentENCurrent[2];
        agentATK[2] = modifiableScript.agentATK[2];
        agentEATK[2] = modifiableScript.agentEATK[2];
        agentDEF[2] = modifiableScript.agentDEF[2];
        agentEDEF[2] = modifiableScript.agentEDEF[2];
        agentSPD[2] = modifiableScript.agentSPD[2];
        agentCritDamage[2] = modifiableScript.agentCritDamage[2];
        agentCritRate[2] = modifiableScript.agentCritRate[2];
        agentPlayerCheck[2] = true;
        #endregion

        //Player - 3
        #region
        agentName[3] = "Gen";
        agentType[3] = 0;

        agentLV[3] = modifiableScript.agentLV[3];
        agentHPMax[3] = modifiableScript.agentHPMax[3];
        agentHPCurrent[3] = modifiableScript.agentHPCurrent[3];
        agentENMax[3] = modifiableScript.agentENMax[3];
        agentENCurrent[3] = modifiableScript.agentENCurrent[3];
        agentATK[3] = modifiableScript.agentATK[3];
        agentEATK[3] = modifiableScript.agentEATK[3];
        agentDEF[3] = modifiableScript.agentDEF[3];
        agentEDEF[3] = modifiableScript.agentEDEF[3];
        agentSPD[3] = modifiableScript.agentSPD[3];
        agentCritDamage[3] = modifiableScript.agentCritDamage[3];
        agentCritRate[3] = modifiableScript.agentCritRate[3];
        agentPlayerCheck[3] = true;
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
