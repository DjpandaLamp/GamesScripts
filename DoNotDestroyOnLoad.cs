using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DoNotDestroyOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Persistant").Length + GameObject.FindGameObjectsWithTag("Player").Length <= 3)
        {
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            GameObject.Destroy(transform.gameObject);
        }
    }



}
