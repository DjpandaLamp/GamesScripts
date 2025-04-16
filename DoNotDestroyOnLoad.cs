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
        }
        else if (transform.tag == "Persistant")
        {
            GameObject.Destroy(transform.gameObject);
        }
        if (GameObject.FindGameObjectsWithTag("MainUI").Length == 1 && transform.tag == "MainUI")
        {
            DontDestroyOnLoad(transform.gameObject);
        }
        else if (transform.tag == "MainUI")
        {
            GameObject.Destroy(transform.gameObject);
        }
        yield return null;
    }
    /*private void FixedUpdate()
    {
        if (GetComponent<Canvas>() != null && tag == "MainUI" && GetComponent<Canvas>().worldCamera == null)
        {
            GetComponent<Canvas>().worldCamera = GameObject.Find("PlayerObject").GetComponentInChildren<Camera>();
        }
    }*/
}
