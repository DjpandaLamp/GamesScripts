using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpawner : MonoBehaviour
{
    public GameObject canvasPrefab;
    private void Awake()
    {
        if (!GameObject.FindWithTag("MainUI"))
        {
            Instantiate(canvasPrefab);
        }
    }
}
