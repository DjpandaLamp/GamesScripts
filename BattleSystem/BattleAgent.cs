using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleAgent : MonoBehaviour
{
    public BattleSystem system;
    public EnemyData data;

    public TextMeshProUGUI agentHeaderText;
    public TextMeshProUGUI agentHealthText;
    public TextMeshProUGUI agentEnergyText;
    public Image agentImage;
    public Image agentBoxImage;
    public Color agentBaseColor;

    public Slider agentHealthSlider;
    public Slider agentEnergySlider;

    public int agentId;
    public int agentType;
    public int agentCount;
    public string agentName;
    public int agentLV;
    public int agentHPMax;
    public int agentHPCurrent;
    public int agentENMax;
    public int agentENCurrent;
    public int agentATKBase;
    public int agentEATKBase;
    public int agentDEFBase;
    public int agentEDEFBase;
    public int agentSPD;


    public int agentATKFull;
    public int agentEATKFull;
    public int agentDEFFull;
    public int agentEDEFFull;

    public bool isDefending;

    public bool HasText = true;

    public bool agentHasGone = false;

    void Start()
    {
        HasText = true;

        system = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        data = GameObject.Find("BattleSystem").GetComponent<EnemyData>();

        Slider[] sliderComponents = GetComponentsInChildren<Slider>();
        agentHealthSlider = sliderComponents[0];
        agentEnergySlider = sliderComponents[1];


        Image[] imageComponents = GetComponentsInChildren<Image>();
        agentImage = imageComponents[0];
        agentBoxImage = imageComponents[1];
        agentBaseColor = imageComponents[1].color;

        TextMeshProUGUI[] textMeshes = GetComponentsInChildren<TextMeshProUGUI>();
        agentHeaderText = textMeshes[0];
        if (textMeshes.Length >= 2)
        {
            agentHealthText = textMeshes[1];
            agentEnergyText = textMeshes[2];
        }
        else
        {
            HasText = false;
        }


        agentImage = GetComponentInChildren<Image>();

        //Grabs Unit Data according to its ID from database
        agentName = data.agentName[agentId];
        agentLV = data.agentLV[agentId];
       // agentImage.sprite = data.agentSprite[agentId];
        agentHPMax= data.agentHPMax[agentId];
        agentHPCurrent = data.agentHPCurrent[agentId];
        agentENMax= data.agentENMax[agentId];
        agentENCurrent= data.agentENCurrent[agentId];
        agentATKBase= data.agentATK[agentId];
        agentEATKBase= data.agentEATK[agentId];
        agentDEFBase= data.agentDEF[agentId];
        agentEDEFBase= data.agentEDEF[agentId];
        agentSPD= data.agentSPD[agentId];


        agentHealthSlider.maxValue = agentHPMax;
        agentHealthSlider.value = agentHPCurrent;
        agentHealthSlider.minValue = 0;

        agentEnergySlider.maxValue = agentENMax;
        agentEnergySlider.value = agentENCurrent;
        agentEnergySlider.minValue = 0;
        
        agentHeaderText.text = agentName + "- LV" + agentLV.ToString();
        if (HasText)
        {
            agentHealthText.text = "Health: " + agentHPCurrent.ToString() + "/" + agentHPMax.ToString();
            agentEnergyText.text = "Energy: " + agentENCurrent.ToString() + "/" + agentENMax.ToString();
        }


}
    void Update()
    {
        UIUpdate(); //Updates Visual Components
        StatUpdate();
    }
    void StatUpdate()
    {
        agentATKFull = agentATKBase;

        agentDEFFull = agentDEFBase;
        if (isDefending)
        {
            agentDEFFull += 20;
        }
        agentEATKFull = agentEATKBase;
        agentEDEFFull = agentEDEFBase;
    }




    void UIUpdate()
    {
        agentHealthSlider.maxValue = agentHPMax;
        agentHealthSlider.value = Mathf.Lerp(agentHealthSlider.value, agentHPCurrent, Time.deltaTime);
        if (agentHealthSlider.value - agentHPCurrent < (agentHPMax / 100) && agentHealthSlider.value - agentHPCurrent > -(agentHPMax / 100))
        {
            agentHealthSlider.value = agentHPCurrent;
        }

        agentEnergySlider.maxValue = agentENMax;
        agentEnergySlider.value = Mathf.Lerp(agentEnergySlider.value, agentENCurrent, Time.deltaTime);
        if (agentEnergySlider.value-agentENCurrent < (agentENMax / 100) && agentEnergySlider.value - agentENCurrent > -(agentENMax/100))
        {
            agentEnergySlider.value = agentENCurrent;
        }
        
        agentHeaderText.text = agentName + " - LV" + agentLV.ToString();
        if (HasText)
        {
            agentHealthText.text = "Health: " + agentHPCurrent.ToString() + "/" + agentHPMax.ToString();
            agentEnergyText.text = "Energy: " + agentENCurrent.ToString() + "/" + agentENMax.ToString();
        }
    }

    public bool TakeDamage(int attackValue)
    {
        if (attackValue - agentDEFFull <= 0)
        {
            agentDEFFull = attackValue-1;
        }
        agentHPCurrent -= attackValue - agentDEFFull;
        
        if(agentHPCurrent <= 0)
        {
            agentHPCurrent = 0;
            return true;
        }
        return false;
    }

    public void ReceiveHeal(int eAttackValue)
    {
        agentHPCurrent += Mathf.RoundToInt(eAttackValue/2.5f);
        if (agentHPCurrent > agentHPMax)
        {
            agentHPCurrent = agentHPMax;
        }
    }
}
