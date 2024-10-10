using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GameObject.FindWithTag("Persistant").GetComponent<SceneLoader>();
    }

    public void MainMenuStartGame()
    {
        sceneLoader.SceneLoaded(3);
        
    }
    public void MainMenuLoadGame()
    {

    }
    public void MainMenuSetting()
    {

    }
    public void MainMenuQuitGame()
    {
        Application.Quit();
    }
}
