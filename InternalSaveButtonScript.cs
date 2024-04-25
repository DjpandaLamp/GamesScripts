using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InternalSaveButtonScript : MonoBehaviour
{
    private JSONSave jSONSave;


    public int saveID;
    public TextMeshProUGUI saveNum;
    public TextMeshProUGUI playtimeText;
    public TextMeshProUGUI timeStampText;
    public TextMeshProUGUI locationName;
    private void Awake()
    {
        jSONSave = GameObject.FindWithTag("Persistant").GetComponent<JSONSave>();
    }

    public void Load()
    {
        jSONSave.LoadFromJSON(0,saveID);
    }
    public void Save()
    {
        jSONSave.SaveToJSON(0);
    }
}
