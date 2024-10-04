using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverButtonScript : MonoBehaviour
{
    public void GoToMenu()
    {
        SceneManager.LoadSceneAsync(0);
        
    }
    public void GoToDesktop()
    {
        Application.Quit();
    }
}
