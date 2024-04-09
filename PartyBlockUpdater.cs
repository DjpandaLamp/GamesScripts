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
    public Image sliderColor;
    public Image sliderBackColor;

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

    private void Awake()
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
            objName.text = data.p1NM;
            objLevel.text = "LV " + data.p1LV.ToString();
            currentHP = data.p1HP;
            maxHP = data.p1MHP;

           

            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;

        }
        if (ID == 2)
        {
            objName.text = data.p2NM;
            objLevel.text = "LV " + data.p2LV.ToString();
            currentHP = data.p2HP;
            maxHP = data.p2MHP;

            

            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
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
            sliderBackColor.color = new Color(sliderBackColor.color.r, sliderBackColor.color.g, sliderBackColor.color.b, transparency);
            sliderColor.color = new Color(sliderColor.color.r, sliderColor.color.g, sliderColor.color.b, transparency);
        }
        else
        {
            if (ID == 1)
            {
                currentEN = data.p1EN;
                maxEN = data.p1MEN;
                enSlider.maxValue = maxEN;
                enSlider.value = currentEN;
                objHPText.text = "HP                    " + currentHP.ToString() + "/" + maxHP.ToString();
                objENText.text = "EN                    " + currentEN.ToString() + "/" + maxEN.ToString();
            }
            if (ID == 2)
            {
                currentEN = data.p2EN;
                maxEN = data.p2MEN;
                enSlider.maxValue = maxEN;
                enSlider.value = currentEN;
                objHPText.text = "HP                    " + currentHP.ToString() + "/" + maxHP.ToString();
                objENText.text = "EN                   " + currentEN.ToString() + "/" + maxEN.ToString();
            }
        }
       
    }
}
