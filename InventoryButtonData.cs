using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryButtonData : MonoBehaviour, IPointerEnterHandler
{
    private OverworldMenuManager BaseMenu;
    public int ID;
    public TMP_Text textCategory;
    public TMP_Text textValue;
    public TMP_Text textName;
    public TMP_Text textCount;

    private void Start()
    {
        BaseMenu = GameObject.FindWithTag("OverMenu").GetComponent<OverworldMenuManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BaseMenu.SetDescription(ID);
    }

}
