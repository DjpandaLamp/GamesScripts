using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PartyBlockUpdater : MonoBehaviour
{

    private GlobalPersistantScript data;
    private PlayerOverworldManager player;
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

    public int currentHP;
    public int maxHP;
    public int currentEN;
    public int maxEN;
    public int ID;

    public float transparency = -0.5f;

    public bool isBig = false;
    public bool isMenu = false;

    private void Start()
    {
        data = GameObject.FindWithTag("Persistant").GetComponent<GlobalPersistantScript>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerOverworldManager>();
        image = GetComponent<Image>();
    }

    



    // Update is called once per frame
    void Update()
    {
        uiUpdate();
    }
    void uiUpdate()
    {
        if (ID == 1)
        {

            currentHP = data.healthPoints[0];
            maxHP = data.maxHealthPoints[0];
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
            currentEN = data.energyPoints[0];
            maxEN = data.maxEnergyPoints[0];
            enSlider.maxValue = maxEN;
            enSlider.value = currentEN;
            if (isBig)
            {
                objName.text = data.partyname[0] + " - LV " + data.levels[0].ToString();
                objHPText.text = "HP " + currentHP.ToString() + "/" + maxHP.ToString();
                objENText.text = "EN " + currentEN.ToString() + "/" + maxEN.ToString();
            }
            else
            {
                objName.text = data.partyname[0];
                objLevel.text = "LV " + data.levels[0].ToString();
            }

        }
        if (ID == 2)
        {

            currentHP = data.healthPoints[1];
            maxHP = data.maxHealthPoints[1];
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
            currentEN = data.energyPoints[1];
            maxEN = data.maxEnergyPoints[1];
            enSlider.maxValue = maxEN;
            enSlider.value = currentEN;
            if (isBig)
            {
                objName.text = data.partyname[1] + " - LV " + data.levels[1].ToString();
                objHPText.text = "HP " + currentHP.ToString() + "/" + maxHP.ToString();
                objENText.text = "EN " + currentEN.ToString() + "/" + maxEN.ToString();
            }
            else
            {
                objName.text = data.partyname[1];
                objLevel.text = "LV " + data.levels[1].ToString();
            }
        }

        if (!isBig)
        {
            if (player.xVector == 0 && player.yVector == 0)
            {

                if (transparency >= 0.9f)
                {
                    transparency = 0.9f;
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
    }
}
