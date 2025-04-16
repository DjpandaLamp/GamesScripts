using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PartyBlockUpdater : MonoBehaviour
{

    private GlobalPersistantScript data;
    public PlayerOverworldManager player;
    public OverworldMenuManager menuManager;

    public Image image;
    public Image photo;
    public Image sliderColorHP;
    public Image sliderBackColorHP;
    public Image sliderColorEN;
    public Image sliderBackColorEN;

    public TextMeshProUGUI objName;
    public TextMeshProUGUI objLevel;
    public TextMeshProUGUI objHPText;
    public TextMeshProUGUI objENText;
    public Slider hpSlider;
    public Slider enSlider;

    public double currentHP;
    public double maxHP;
    public double currentEN;
    public double maxEN;
    public int ID;

    public float transparency = -0.5f;

    public bool isBig = false;
    public bool isMenu = false;

    private void Start()
    {
        data = GameObject.FindWithTag("Persistant").GetComponent<GlobalPersistantScript>();
        image = GetComponent<Image>();
        if (GameObject.Find("PlayerObject") != null) 
        {
            player = GameObject.Find("PlayerObject").GetComponent<PlayerOverworldManager>();
        }
 
    }

    



    // Update is called once per frame
    void Update()
    {
        uiUpdate();
    }
    void uiUpdate()
    {
        if (player != null)
        {
            currentHP = data.healthPoints[ID];
            maxHP = data.maxHealthPoints[ID];
            hpSlider.maxValue = (float)maxHP;
            hpSlider.value = (float)currentHP;
            currentEN = data.energyPoints[ID];
            maxEN = data.maxEnergyPoints[ID];
            enSlider.maxValue = (float)maxEN;
            enSlider.value = (float)currentEN;
            if (isBig)
            {
                objName.text = data.partyname[ID] + " - LV " + data.levels[0].ToString();
                objHPText.text = "HP " + currentHP.ToString() + "/" + maxHP.ToString();
                objENText.text = "EN " + currentEN.ToString() + "/" + maxEN.ToString();
            }
            else
            {
                objName.text = data.partyname[ID];
                objLevel.text = "LV " + data.levels[0].ToString();
            }


            if (!isBig)
            {
                if (player.xVector == 0 && player.yVector == 0)
                {

                    if (transparency >= 0.99f)
                    {
                        transparency = 1f;
                    }
                    else
                    {
                        transparency += 0.5f * Time.deltaTime;
                    }
                }
                else
                {
                    if (transparency <= -0.5f)
                    {
                        transparency = -0.5f;
                    }
                    else
                    {
                        transparency -= 2.5f * Time.deltaTime;
                    }
                }
                if (menuManager.isUp)
                {
                    transparency = -0.5f;
                }

                image.color = new Color(image.color.r, image.color.g, image.color.b, transparency);
                objName.color = new Color(objName.color.r, objName.color.g, objName.color.b, transparency);
                objLevel.color = new Color(objLevel.color.r, objLevel.color.g, objLevel.color.b, transparency);
                photo.color = new Color(photo.color.r, photo.color.g, photo.color.b, transparency);
                sliderBackColorHP.color = new Color(sliderBackColorHP.color.r, sliderBackColorHP.color.g, sliderBackColorHP.color.b, transparency);
                sliderColorHP.color = new Color(sliderColorHP.color.r, sliderColorHP.color.g, sliderColorHP.color.b, transparency);
                sliderBackColorEN.color = new Color(sliderBackColorEN.color.r, sliderBackColorEN.color.g, sliderBackColorEN.color.b, transparency);
                sliderColorEN.color = new Color(sliderColorEN.color.r, sliderColorEN.color.g, sliderColorEN.color.b, transparency);
            }
            else
            {
                if (GameObject.Find("PlayerObject") != null)
                {
                    player = GameObject.Find("PlayerObject").GetComponent<PlayerOverworldManager>();
                }
            }
        }
    }
}
