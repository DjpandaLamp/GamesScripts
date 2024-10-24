using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GlobalPersistantScript global;
    public void SceneLoaded(int buildIndex)
    {
        SceneManager.LoadSceneAsync(buildIndex);
        global.OverworldUIChecker();
    }
}
