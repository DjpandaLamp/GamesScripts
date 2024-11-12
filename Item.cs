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

public enum TagGroups
{
    Armor,
    Consumable,
    Weapon
}

public enum status
{
    None,
    All,
    Poison,
    Winded,
    Tired
}

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string displayName;
    public int iD;
    public ItemTag ItemTag;
    public TagGroups tagGroup;
    public int ArmorValue;
    public int ArmorRestist;

    public int healthValue;
    public status status;
    public string desc;
}
