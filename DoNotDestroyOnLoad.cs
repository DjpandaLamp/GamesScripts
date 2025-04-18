using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DoNotDestroyOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.03f);
        if (GameObject.FindGameObjectsWithTag("Persistant").Length == 1 && transform.tag == "Persistant")
        {
            DontDestroyOnLoad(transform.gameObject);
            gameObject.name = "PersistantObject_DDOL";
        }
        else if (transform.tag == "Persistant")
        {
            GameObject.Destroy(transform.gameObject);
        }
        /*if (GameObject.FindGameObjectsWithTag("MainUI").Length == 1 && transform.tag == "MainUI")
        {
            DontDestroyOnLoad(transform.gameObject);
            //gameObject.GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
        else if (transform.tag == "MainUI")
        {
            GameObject.Destroy(transform.gameObject);
        }*/
        yield return null;
    }
}
