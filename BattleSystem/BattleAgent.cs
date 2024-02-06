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
    public int agentATK;
    public int agentDEF;
    public int agentSPD;

    void Start()
    {
        system = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        data = GameObject.Find("BattleSystem").GetComponent<EnemyData>();

        Slider[] sliderComponents = GetComponentsInChildren<Slider>();
        agentHealthSlider = sliderComponents[0];
        agentEnergySlider = sliderComponents[1];

        TextMeshProUGUI[] textMeshes = GetComponentsInChildren<TextMeshProUGUI>();
        agentHeaderText = textMeshes[0];
        agentHealthText = textMeshes[1];
        agentEnergyText = textMeshes[2];

        agentImage = GetComponentInChildren<Image>();

        //Grabs Unit Data according to its ID from database
        agentName = data.agentName[agentId];
        agentLV = data.agentLV[agentId];
        agentImage.sprite = data.agentSprite[agentId];
        agentHPMax= data.agentHPMax[agentId];
        agentHPCurrent = data.agentHPCurrent[agentId];
        agentENMax= data.agentENMax[agentId];
        agentENCurrent= data.agentENCurrent[agentId];
        agentATK= data.agentATK[agentId];
        agentDEF= data.agentDEF[agentId];
        agentSPD= data.agentSPD[agentId];


        agentHealthSlider.maxValue = agentHPMax;
        agentHealthSlider.value = agentHPCurrent;
        agentHealthSlider.minValue = 0;

        agentEnergySlider.maxValue = agentENMax;
        agentEnergySlider.value = agentENCurrent;
        agentEnergySlider.minValue = 0;

        agentHeaderText.text = agentName + "- LV" + agentLV.ToString();
        agentHealthText.text = "Health: " + agentHPCurrent.ToString() + "/" + agentHPMax.ToString();
        agentEnergyText.text = "Energy: " + agentENCurrent.ToString() + "/" + agentENMax.ToString();

}
    void Update()
    {
        UIUpdate(); //Updates Visual Components
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
        agentHealthText.text = "Health: " + agentHPCurrent.ToString() + "/" + agentHPMax.ToString();
        agentEnergyText.text = "Energy: " + agentENCurrent.ToString() + "/" + agentENMax.ToString();
    }
}
