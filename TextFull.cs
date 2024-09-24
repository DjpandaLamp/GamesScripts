using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFull : MonoBehaviour
{
   
    public string[] strings;
    public Vector2[] faces;

    public Sprite[] sprite; //Character face, emotion

    /*
     * This is the Error Index, Please refer to this if you come across any errors.
     * TX0: The strings[] index equals zero
     * TX1: The strings[i] value is not set
     * 
    */

    // Start is called before the first frame update
    void Start()
    {
        strings = new string[600];
        faces = new Vector2[600];
        sprite = new Sprite[600];


        for (int i = 0; i < strings.Length; i++)
        {
            strings[i] = "This is an Error String Index #" + i.ToString() + ". Please report this to the game dev for Error Code TX1.";
            faces[i] = Vector2.zero;
            
        }

        strings[0] = "Yo, This is an Error Message. If you are seeing this, please inform the developer for Error Code TX0. Thank you.";
        strings[1] = "A Legendary Item, Known to all as 'The Xenoblade'. Handle with Care.";
        strings[2] = "No Name Assigned";
        strings[3] = "A Mythical skill, Known to all as 'Starsearing Flame'. Wield with Care.";
        strings[4] = "0";
        strings[19] = "0";
        strings[20] = "Use the Arrow Keys to Move. Use the Left Mouse Button or Z to close this text box. Press Esc to open the Menu";
        faces[20] = new Vector2(0, 1);
        strings[21] = "To quit the application, Press the Quit Button. To Save Press New Save on the Save Menu";
        faces[21] = new Vector2(0, 2);
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
