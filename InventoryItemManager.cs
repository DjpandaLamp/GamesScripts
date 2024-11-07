using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemManager : MonoBehaviour
{


    public Item[] items;
    public int[] itemCounts;

    public void addItem(int ItemId)
    {
        itemCounts[ItemId] += 1;
    }
    public void ridItem(int ItemId)
    {
        itemCounts[ItemId] -= 1;
    }
}
