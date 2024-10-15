using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DoNotDestroyOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Persistant").Length == 1 && transform.tag == "Persistant")
        {
            DontDestroyOnLoad(transform.gameObject);
        }
        else if (transform.tag == "Persistant")
        {
            GameObject.Destroy(transform.gameObject);
        }
        if (GameObject.FindGameObjectsWithTag("MainUI").Length >= 1 && transform.tag == "MainUI")
        {
            DontDestroyOnLoad(transform.gameObject);
        }
        else if (transform.tag == "MainUI")
        {
            GameObject.Destroy(transform.gameObject);
        }

    }



}
