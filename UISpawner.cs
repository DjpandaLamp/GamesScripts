using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISpawner : MonoBehaviour
{
    public GameObject canvasPrefab;


    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }


    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0 || scene.buildIndex != 1 || scene.buildIndex != 2)
        {
            if (!GameObject.FindWithTag("MainUI"))
            {
                if (scene.buildIndex == 1 || scene.buildIndex == 0 || scene.buildIndex == 2)
                {
                    Debug.Log("Scene Index Not Ready For Game Menu Instantiation");
                }
                else
                {
                    Debug.Log("Game Menu Instantiation");
                    Instantiate(canvasPrefab);
                }

            }
        }
    }
}
