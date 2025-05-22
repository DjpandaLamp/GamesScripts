using CartoonFX;
using Coffee.UIExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Mathfunctions;
using System.Runtime.CompilerServices;

public class BattleAgent : MonoBehaviour, IComparable
{
    public BattleSystem system;
    public EnemyData data;
    public ShakeTransformS shake;

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
    public double agentHPMax;
    public double agentHPCurrent;
    public double agentENMax;
    public double agentENCurrent;
    public double agentATKBase;
    public double agentEATKBase;
    public double agentDEFBase;
    public double agentEDEFBase;
    public double agentSPD;
    public double agentCritRate;
    public double agentCritDamage;

    public int currentBattleSpeedIndex;

    public double agentATKFull;
    public double agentEATKFull;
    public double agentDEFFull;
    public double agentEDEFFull;

    public UIParticle agentUIParticle;
    public ParticleSystem agentParticleSystem;
    public AudioSource audioSource;
    public PulseImage pulse;

    public bool noEN;

    public bool isDefending;

    public bool HasText = true;

    public bool agentHasGone = false;

    private BattleSystemStaticElementReferenceScript referenceScript;

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
        referenceScript = GameObject.Find("Static Ref Object").GetComponent<BattleSystemStaticElementReferenceScript>();
        system = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        data = GameObject.Find("BattleSystem").GetComponent<EnemyData>();
        audioSource = GetComponent<AudioSource>();
        shake = GetComponent<ShakeTransformS>();

        Slider[] sliderComponents = GetComponentsInChildren<Slider>();
        agentHealthSlider = sliderComponents[0];
        if (!noEN)
        {
            agentEnergySlider = sliderComponents[1];
        }

        agentHealthFillRect = agentHealthSlider.fillRect.GetComponent<Image>();
        agentHealthSliderBaseColor = agentHealthFillRect.color;

        Image[] imageComponents = GetComponentsInChildren<Image>();
        agentImage = imageComponents[1];
        agentBoxImage = imageComponents[0];
        agentBaseColor = imageComponents[0].color; 

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

        if (agentId != 0)
        {
            agentImage = GetComponentsInChildren<Image>()[1];
        }
        else
        {
            agentImage = GetComponentsInChildren<Image>()[0];
        }
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
        agentCritDamage = data.agentCritDamage[agentId];
        agentCritRate = data.agentCritRate[agentId];



        agentHealthSlider.maxValue = (float)agentHPMax;
        agentHealthSlider.value = (float)agentHPCurrent;
        agentHealthSlider.minValue = 0;
        if (!noEN)
        {
            agentEnergySlider.maxValue = (float)agentENMax;
            agentEnergySlider.value = (float)agentENCurrent;
            agentEnergySlider.minValue = 0;
        }


        if (!data.agentPlayerCheck[agentId])
        {
            agentHPMax = (int)Math.Round(agentHPMax*UnityEngine.Random.Range(0.9f,1.1f));
            agentENMax = (int)Math.Round(agentENMax * UnityEngine.Random.Range(0.9f, 1.1f));
            agentHPCurrent = agentHPMax;
            agentENCurrent = agentENMax;
            agentATKBase = (int)Math.Round(agentATKBase * UnityEngine.Random.Range(0.9f, 1.1f));
            agentEATKBase = (int)Math.Round(agentEATKBase * UnityEngine.Random.Range(0.9f, 1.1f));
            agentDEFBase = (int)Math.Round(agentDEFBase * UnityEngine.Random.Range(0.9f, 1.1f));
            agentEDEFBase = (int)Math.Round(agentEDEFBase * UnityEngine.Random.Range(0.9f, 1.1f));
            agentSPD = (int)Math.Round(agentSPD * UnityEngine.Random.Range(0.9f, 1.1f));
            
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
        if (pulse != null)
        {
            if (system.currentActiveAgent.GetComponent<BattleAgent>() == GetComponent<BattleAgent>())
            {

                
                pulse.AlphaStaticTarget = 0.8f;

            }
            else
            {
                pulse.AlphaStaticTarget = 0.25f;
            }
        }
    }

    


    void UIUpdate()
    {
        agentHealthSlider.maxValue = (float)agentHPMax;
        agentHealthSlider.value = Mathf.Lerp(agentHealthSlider.value, (float)agentHPCurrent, Time.deltaTime);
        if (agentHealthSlider.value - agentHPCurrent < (agentHPMax / 100) && agentHealthSlider.value - agentHPCurrent > -(agentHPMax / 100))
        {
            agentHealthSlider.value = (float)agentHPCurrent;
        }
        if (!noEN)
        {
            agentEnergySlider.maxValue = (float)agentENMax;
            agentEnergySlider.value = Mathf.Lerp(agentEnergySlider.value, (float)agentENCurrent, Time.deltaTime);
            if (agentEnergySlider.value - agentENCurrent < (agentENMax / 100) && agentEnergySlider.value - agentENCurrent > -(agentENMax / 100))
            {
                agentEnergySlider.value = (float)agentENCurrent;
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
        if (system.state == battleStateMachine.PlayerEnemyTargeting)
        {
            int pos = 0;
            
            for (int i = 0; i < system.EnemyArray.Length; i++)
            {
                if (GetComponent<BattleAgent>() == system.EnemyArray[i].GetComponent<BattleAgent>())
                {
                    pos = i;
                    
                    break;
                }
            }
                system.EnemyArray[system.playerCursorPos].GetComponent<BattleAgent>().agentHealthFillRect.color = system.EnemyArray[system.playerCursorPos].GetComponent<BattleAgent>().agentHealthSliderBaseColor;
                system.playerCursorPos = pos;

        }
        if (system.state == battleStateMachine.PlayerPlayerTargeting)
        {
            int pos = 0;
            
            for (int i = 0; i < system.PlayerArray.ToArray().Length; i++)
            {
                if (GetComponent<BattleAgent>() == system.PlayerArray[i].GetComponent<BattleAgent>())
                {
                    pos = i;
                    
                    break;
                }
            }
            system.PlayerArray[system.playerCursorPos].GetComponent<BattleAgent>().agentHealthFillRect.color = system.PlayerArray[system.playerCursorPos].GetComponent<BattleAgent>().agentHealthSliderBaseColor;
            system.playerCursorPos = pos;
        }
    }

    public Vector2 TakeDamage(double attackValue, float RateValue, float DamageValue, int damageType)
    {
        int isDefeated = 0;
        bool hasNotCrit = false;
        int critLevel = 0;

        while(!hasNotCrit) //Critcal Loop
        {
            if ((RateValue/Math.Pow(2,critLevel+1)) > UnityEngine.Random.Range(0,101))
            {
                critLevel += 1;
                continue;
            }
            else
            {
                hasNotCrit = true;
            }
        }
        double tempAgentDEFFull = agentDEFFull;
        if (Math.Round(attackValue * Mathf.Pow(DamageValue, critLevel)) - agentDEFFull <= 0) // If final damage is below 1, set defense to attack-1
        {
            tempAgentDEFFull = Math.Floor((attackValue * (Mathf.Pow(DamageValue, critLevel)))) - 1;
            agentHPCurrent -= Math.Round(attackValue * (Mathf.Pow(DamageValue, critLevel))) - tempAgentDEFFull;
        }
        else //Regular Damage Formula
        {
            agentHPCurrent -= Math.Round(attackValue * (Mathf.Pow(DamageValue, critLevel))) - agentDEFFull;
        }

        AudioClip clip = null;
        Color color1New = Color.white;
        Color color2New = Color.green;
        Color colorBGNew = Color.white;
        if (agentHPMax * 0.25f < Math.Round(attackValue * (Mathf.Pow(DamageValue, critLevel))) - agentDEFFull)
        {
            
            switch (damageType)
            {
                case 0: //Physical
                    clip = referenceScript.attackGenericAverage;
                    color1New = Color.white;
                    color2New = Color.white;
                    colorBGNew = Color.grey;
                    break;
                case 1: //Fire
                    clip = referenceScript.attackFireAverage;
                    color1New = Color.white;
                    color2New = Color.red;
                    colorBGNew = new Color(0.5f,0,0,1);
                    break;
                case 2: //Water
                    //clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/SFX/BattleHeavyHit.wav", typeof(AudioClip));
                    break;
                case 3: //Earth
                   // clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/SFX/BattleHeavyHit.wav", typeof(AudioClip));
                    break;
                case 4: //Psysic
                    clip = referenceScript.attackPsysicAverage;
                    break;
                case 5: //Poison
                    //clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/SFX/BattleHeavyHit.wav", typeof(AudioClip));
                    break;
                case 6: //Light
                    //clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/SFX/BattleHeavyHit.wav", typeof(AudioClip));
                    break;
                case 7: //Dark
                   // clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/SFX/BattleHeavyHit.wav", typeof(AudioClip));
                    break;
            }      
            audioSource.clip = clip;
        }
        if (agentHPMax * 0.5f < Math.Round(attackValue * (Mathf.Pow(DamageValue, critLevel))) - agentDEFFull)
        {

            switch (damageType)
            {
                case 0: //Physical
                    clip = referenceScript.attackGenericStrong;
                    break;
                case 1: //Fire
                    clip = referenceScript.attackFireStrong;
                    break;
                case 2: //Water
                        //clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/SFX/BattleHeavyHit.wav", typeof(AudioClip));
                    break;
                case 3: //Earth
                        // clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/SFX/BattleHeavyHit.wav", typeof(AudioClip));
                    break;
                case 4: //Psysic
                    clip = referenceScript.attackPsysicStrong;
                    break;
                case 5: //Poison
                        //clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/SFX/BattleHeavyHit.wav", typeof(AudioClip));
                    break;
                case 6: //Light
                        //clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/SFX/BattleHeavyHit.wav", typeof(AudioClip));
                    break;
                case 7: //Dark
                        // clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/SFX/BattleHeavyHit.wav", typeof(AudioClip));
                    break;
            }
            audioSource.clip = clip;
        }
        if (agentHPCurrent <= 0)
        {
            //clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sound/SFX/EnemyDefeated.wav", typeof(AudioClip));
            audioSource.clip = clip;
            agentHPCurrent = 0;
            isDefeated = 1;
        }

        CFXR_ParticleText _ParticleText = agentParticleSystem.GetComponent<CFXR_ParticleText>();
        _ParticleText.color1 = color1New;
        _ParticleText.color2 = color2New;
        _ParticleText.backgroundColor = colorBGNew;
        _ParticleText.UpdateText((Math.Round(attackValue * (Mathf.Pow(DamageValue, critLevel))) - tempAgentDEFFull).ToString());

        agentParticleSystem.Play();
        audioSource.Play();
        agentUIParticle.RefreshParticles();
        shake.Begin();

        return new Vector2(isDefeated, critLevel);
    }

    public void ReceiveHeal(double eAttackValue)
    {
        agentHPCurrent += Math.Round(eAttackValue/2.5f);
        if (agentHPCurrent > agentHPMax)
        {
            agentHPCurrent = agentHPMax;
        }
    }
}
