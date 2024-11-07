using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTag
{
    ArmorHead,
    ArmorSocks,
    ArmorShoes,
    ArmorPants,
    ArmorGloves,
    ArmorRing,
    ArmorHelmet,
    ArmorShirt,
    ConsumableHeal,
    ConsumableStatus,
    Weapon
}
[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public int iD;
    public ItemTag ItemTag;
    public int ArmorValue;
    public int ArmorRestist;

    public int healthValue;
    public int status;
    public string desc;
}
