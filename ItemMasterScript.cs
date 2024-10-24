using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMasterScript : MonoBehaviour
{
    public enum objecttype
    {
        Food,
        Armor,
        Weapons,
        Key_Items,
        SevenShards
    }
    public int[] healthValue;
    public int[] DamageValue;
    public int[] ArmorValue;
    public int[] Effect;
    public int[] EffectValue;
    public objecttype[] objecttypes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
