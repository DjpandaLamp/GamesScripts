using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextCaller : MonoBehaviour
{
    public int index;
    public TextStartEnd textSE;
    public int type;
    public int id;

    private void Awake()
    {
        textSE = GameObject.FindObjectOfType<TextStartEnd>(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (textSE != null)
        {
            if (collision.gameObject.GetComponent<PlayerOverworldManager>() != null)
            {

                textSE.gameObject.SetActive(true);
                textSE.CallTextWriter(index);
                gameObject.SetActive(false);


            }
        }

    }


}
