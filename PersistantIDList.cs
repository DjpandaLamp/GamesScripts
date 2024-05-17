using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantIDList : MonoBehaviour
{
    public string[] acceptedIDs;
    public int preset;
    //00000 - Player
    //10000 - Overworld Menu UI
    //01000 - Overworld Clock and Partyboxes
    //11000 - Persistant Object

    private void Awake()
    {
        if (preset == 0) // Custom
        {
            
        }

        if (preset == 1) // Overworld Default
        {
            acceptedIDs = new string[4] { "00000", "10000", "01000", "11000" };
        }
        if (preset == 2) // Battle Default
        {
            acceptedIDs = new string[1] { "11000" };
        }
    }

}
