using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{

    public void MainMenuStartGame()
    {
        SceneManager.LoadSceneAsync(2);
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
