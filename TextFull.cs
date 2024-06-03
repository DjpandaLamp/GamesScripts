using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFull : MonoBehaviour
{
   
    public string[] strings;

    // Start is called before the first frame update
    void Start()
    {
        strings = new string[4000];
        

        for (int i = 0; i < strings.Length; i++)
        {
            strings[i] = "This is an Error String Index #" + i.ToString() + ". Please report this to the game dev for Error Code TX1.";
            
        }

        strings[0] = "Yo, This is an Error Message. If you are seeing this, please inform the developer for Error Code TX0. Thank you.";
        strings[1] = "A Legendary Item, Known to all as 'The Dev forgot to put a description on it'. Handle with Care.";
        strings[2] = "No Name Assigned";
        strings[3] = "A Mythical skill, Known to all as 'The Dev forgot to put a description on it'. Wield with Care.";
        strings[4] = "0";
        strings[19] = "0";
        strings[20] = "Use the Arrow Keys to Move. Use the Left Mouse Button or Z to close this text box. Press Esc to open the Menu";
        strings[21] = "To quit the application, Press the Quit Button. To Save Press New Save on the Save Menu";
        strings[22] = "0";
        strings[499] = "0";
        strings[500] = "Wow, Those walls sure do look walkable!";
        strings[501] = "0";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
