using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillPopulateScript : MonoBehaviour
{
    public BattleSystem system;

    public int spellIndex;
    public TextMeshProUGUI spellText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI typeText;

    public void setButton()
    {
        GetComponent<Button>().onClick.AddListener(delegate { system.CallSkillPublic(spellIndex); });
    }

}
