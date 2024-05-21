using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextCaller : MonoBehaviour
{
    public int index;
    public TextStartEnd textSE;


    private void Awake()
    {
        StartCoroutine(SetRef());
       
    }


    IEnumerator SetRef()
    {
        while (textSE == null)
        {
            yield return new WaitForSeconds(0.5f);
            textSE = GameObject.Find("TextObject").GetComponent<TextStartEnd>();
             
        }

        
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
