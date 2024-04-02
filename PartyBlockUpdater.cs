using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PartyBlockUpdater : MonoBehaviour
{

    private GlobalPersistantScript data;
    public Image Image;
    public TextMeshProUGUI objName;
    public TextMeshProUGUI objLevel;
    public int currentHP;
    public int maxHP;
    public int ID;

    private void Awake()
    {
        data = GameObject.FindWithTag("Persistant").GetComponent<GlobalPersistantScript>();
        
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
        }
        if (ID == 2)
        {
            objName.text = data.p2NM;
            objLevel.text = "LV " + data.p2LV.ToString();
            currentHP = data.p2HP;
            maxHP = data.p2MHP;
        }
    }
}
