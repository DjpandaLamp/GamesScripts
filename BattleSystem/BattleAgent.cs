using CartoonFX;
using Coffee.UIExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mathfunctions;
using System.Runtime.CompilerServices;

public class BattleAgent : MonoBehaviour, IComparable
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
    public Image agentHealthFillRect;
    public Color agentHealthSliderBaseColor;

    public Slider agentEnergySlider;

    public bool agentIdentity;
    public int agentId;
    public int agentType;
    public int agentCount;
    public string agentName;
    public int agentLV;
    public float agentHPMax;
    public float agentHPCurrent;
    public float agentENMax;
    public float agentENCurrent;
    public float agentATKBase;
    public float agentEATKBase;
    public float agentDEFBase;
    public float agentEDEFBase;
    public float agentSPD;

    public int currentBattleSpeedIndex;

    public float agentATKFull;
    public float agentEATKFull;
    public float agentDEFFull;
    public float agentEDEFFull;

    public UIParticle agentUIParticle;
    public ParticleSystem agentParticleSystem;

    public bool noEN;

    public bool isDefending;

    public bool HasText = true;

    public bool agentHasGone = false;

    public int CompareTo(object obj)
    {
        var a = this;
        var b = obj as BattleAgent;

        if (a.agentSPD < b.agentSPD)
            return 1;

        if (a.agentSPD > b.agentSPD)
            return -1;

        return 0;
    }

    void Start()
    {
        HasText = true;

        system = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        data = GameObject.Find("BattleSystem").GetComponent<EnemyData>();

        Slider[] sliderComponents = GetComponentsInChildren<Slider>();
        agentHealthSlider = sliderComponents[0];
        if (!noEN)
        {
            agentEnergySlider = sliderComponents[1];
        }

        agentHealthFillRect = agentHealthSlider.fillRect.GetComponent<Image>();
        agentHealthSliderBaseColor = agentHealthFillRect.color;

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
        agentImage.sprite = data.agentSprite[agentId];
         
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
        if (!noEN)
        {
            agentEnergySlider.maxValue = agentENMax;
            agentEnergySlider.value = agentENCurrent;
            agentEnergySlider.minValue = 0;
        }


        if (!data.agentPlayerCheck[agentId])
        {
            agentHPMax = (int)MathF.Round(agentHPMax*UnityEngine.Random.Range(0.9f,1.1f));
            agentENMax = (int)MathF.Round(agentENMax * UnityEngine.Random.Range(0.9f, 1.1f));
            agentHPCurrent = agentHPMax;
            agentENCurrent = agentENMax;
            agentATKBase = (int)MathF.Round(agentATKBase * UnityEngine.Random.Range(0.9f, 1.1f));
            agentEATKBase = (int)MathF.Round(agentEATKBase * UnityEngine.Random.Range(0.9f, 1.1f));
            agentDEFBase = (int)MathF.Round(agentDEFBase * UnityEngine.Random.Range(0.9f, 1.1f));
            agentEDEFBase = (int)MathF.Round(agentEDEFBase * UnityEngine.Random.Range(0.9f, 1.1f));
            agentSPD = (int)MathF.Round(agentSPD * UnityEngine.Random.Range(0.9f, 1.1f));
            
        }

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
        if (!noEN)
        {
            agentEnergySlider.maxValue = agentENMax;
            agentEnergySlider.value = Mathf.Lerp(agentEnergySlider.value, agentENCurrent, Time.deltaTime);
            if (agentEnergySlider.value - agentENCurrent < (agentENMax / 100) && agentEnergySlider.value - agentENCurrent > -(agentENMax / 100))
            {
                agentEnergySlider.value = agentENCurrent;
            }
        }

        
        agentHeaderText.text = agentName + " - LV" + agentLV.ToString();
        if (HasText)
        {
            agentHealthText.text = "Health: " + agentHPCurrent.ToString() + "/" + agentHPMax.ToString();
            agentEnergyText.text = "Energy: " + agentENCurrent.ToString() + "/" + agentENMax.ToString();
        }
    }

    private void OnMouseOver()
    {
        if (system.state == battleStateMachine.PlayerTargeting)
        {
            int pos = 0;
            bool player = true;
            for (int i = 0; i < system.EnemyArray.Length; i++)
            {
                if (GetComponent<BattleAgent>() == system.EnemyArray[i].GetComponent<BattleAgent>())
                {
                    pos = i;
                    player = false;
                    break;
                }
            }
            if (player == false)
            {
                system.EnemyArray[system.playerCursorPos].GetComponent<BattleAgent>().agentHealthFillRect.color = system.EnemyArray[system.playerCursorPos].GetComponent<BattleAgent>().agentHealthSliderBaseColor;
                system.playerCursorPos = pos;
            }
        }
    }

    public bool TakeDamage(float attackValue)
    {
        if (attackValue - agentDEFFull <= 0)
        {
            agentDEFFull = attackValue-1;
        }
        agentHPCurrent -= attackValue - agentDEFFull;

        CFXR_ParticleText _ParticleText = agentParticleSystem.GetComponent<CFXR_ParticleText>();
        _ParticleText.UpdateText((attackValue-agentDEFFull).ToString());
        agentParticleSystem.Play();
        agentUIParticle.RefreshParticles();


        if(agentHPCurrent <= 0)
        {
            agentHPCurrent = 0;
            return true;
        }
        return false;
    }

    public void ReceiveHeal(float eAttackValue)
    {
        agentHPCurrent += Mathf.RoundToInt(eAttackValue/2.5f);
        if (agentHPCurrent > agentHPMax)
        {
            agentHPCurrent = agentHPMax;
        }
    }
}
