using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct SpellData
{
    public string spellName;
    public bool isDamageorHeal; //0 is damage, 1 is heal
    public float dmgRatio; //multiplier to the agent's damage value
    public int hitNumber; //Number of Times it hits
    public int dmgType; //elemental damage type (leave blank if heal)
    public int fixedCritRate; // Leave null if you don't want a fixed crit rate
    public int inflictedStatus; //leave 0 if no inflicted status * 1 is poison * 2 is burn * 3 is soggy * 4 is dazed * 5 is buried * 6 is healing aura * 7 is attack+ * 8 is attack- * 9 is def+ * 10 is def-
    public int ENCost;
}


public class EnemyData : MonoBehaviour
{
    private int count = 20;

    public PlayerDataModifiableScript modifiableScript;

    public string[] agentName; //Name of Enemy
    public int[] agentType; //Basic or Boss Enemy
    public Sprite[] agentSprite; //Sprite of the Enemy

    public SpellData[] spells;

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
        spells = new SpellData[25];

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

        //ENEMY DATA
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


        Debug.Log("Spells Loading");
        //SPELL DATA

        //Healing Spell 1
        spells[0].spellName = "Quick Patch";
        spells[0].isDamageorHeal = true;
        spells[0].dmgRatio = 0.5f;
        spells[0].ENCost = 8;

        //Healing Spell 2
        spells[1].spellName = "Flesh Fixer";
        spells[1].isDamageorHeal = true;
        spells[1].dmgRatio = 1f;
        spells[1].ENCost = 12;

        //Healing Spell 3
        spells[2].spellName = "Refactor Body";
        spells[2].isDamageorHeal = true;
        spells[2].dmgRatio = 2f;
        spells[2].ENCost = 20;

        //Healing Spell 4
        spells[3].spellName = "Rework Self";
        spells[3].isDamageorHeal = true;
        spells[3].dmgRatio = 4f;
        spells[3].ENCost = 36;

        //Healing Aura 1
        spells[4].spellName = "Healing Maintenance";
        spells[4].isDamageorHeal = true;
        spells[4].dmgRatio = 0.25f;
        spells[4].inflictedStatus = 6;
        spells[4].ENCost = 22;

        //Healing Aura 2
        spells[5].spellName = "Squashing Zone";
        spells[5].isDamageorHeal = true;
        spells[5].dmgRatio = 0.50f;
        spells[5].inflictedStatus = 6;
        spells[5].ENCost = 46;

        //Healing Aura 3
        spells[6].spellName = "Redundacy Crusher";
        spells[6].isDamageorHeal = true;
        spells[6].dmgRatio = 1f;
        spells[6].inflictedStatus = 6;
        spells[6].ENCost = 122;

        //Physical Spell 1
        spells[14].spellName = "Hash it Out";
        spells[14].isDamageorHeal = false;
        spells[14].dmgRatio = 1.2f;
        spells[14].hitNumber = 1;
        spells[14].dmgType = 0;
        spells[14].inflictedStatus = 0;
        spells[14].ENCost = 8;

        //Physical Multi Spell 1
        spells[15].spellName = "Punchy Punchy Array";
        spells[15].isDamageorHeal = false;
        spells[15].dmgRatio = 0.8f;
        spells[15].hitNumber = 3;
        spells[15].dmgType = 0;
        spells[15].inflictedStatus = 0;
        spells[15].ENCost = 18;

        //Physical Spell 2
        spells[16].spellName = "Variable Strike";
        spells[16].isDamageorHeal = false;
        spells[16].dmgRatio = 2.2f;
        spells[16].hitNumber = 1;
        spells[16].dmgType = 0;
        spells[16].inflictedStatus = 0;
        spells[16].ENCost = 18;

        //Physical Multi Spell 2
        spells[17].spellName = "List of Grievences";
        spells[17].isDamageorHeal = false;
        spells[17].dmgRatio = 1f;
        spells[17].hitNumber = 5;
        spells[17].dmgType = 0;
        spells[17].inflictedStatus = 0;
        spells[17].ENCost = 50;

        //Physical Spell 3
        spells[18].spellName = "Classic Slice";
        spells[18].isDamageorHeal = false;
        spells[18].dmgRatio = 5.2f;
        spells[18].hitNumber = 1;
        spells[18].dmgType = 0;
        spells[18].inflictedStatus = 0;
        spells[18].ENCost = 35;

        //Physical Multi Spell 3
        spells[19].spellName = "String of Strikes";
        spells[19].isDamageorHeal = false;
        spells[19].dmgRatio = 1.6f;
        spells[19].hitNumber = 5;
        spells[19].dmgType = 0;
        spells[19].inflictedStatus = 0;
        spells[19].ENCost = 72;


    }

    public SpellData CallSkill(int skillIndex)
    {
        return spells[skillIndex];
    }
}
