using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONSave : MonoBehaviour
{
    private PlayerOverworldManager PlayerOverworldManager;
    private GlobalPersistantScript persistantScript;
    private string persistantPath;

    public Config config;
    public Save Save;
    private void Start()
    {
        persistantPath = Application.persistentDataPath;
        PlayerOverworldManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOverworldManager>();
    }
    private void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveToJSON(2, 0);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadFromJSON();
        }
    }




    void SaveToJSON(int type, int num)
    {
        if (type == 0) //Main Save
        {
            Save.timestamp = System.DateTime.Now.ToString();
            Save.timeElapsed += persistantScript.globalTimeElapsed;
            persistantScript.globalTimeElapsed = 0;






            string SaveData = JsonUtility.ToJson(Save);
            string filePath = persistantPath + "/SaveData" + num.ToString() + ".json";
            Debug.Log(filePath);
            System.IO.File.WriteAllText(filePath, SaveData);
        }
        if (type == 1) //Auto File
        {
            
        }
        if (type == 2) //Settings Autosave
        {
            string configData = JsonUtility.ToJson(config);
            string filePath = persistantPath + "/ConfigData.json";
            Debug.Log(filePath);
            System.IO.File.WriteAllText(filePath, configData);
        }
        Debug.Log("Successfully Saved");
    }
    void LoadFromJSON()
    {
        
        string filePath = persistantPath + "/ConfigData.json";

        string configData = System.IO.File.ReadAllText(filePath);
        config = JsonUtility.FromJson<Config>(configData);

        Debug.Log("Successfully Loaded");
    }
    
}


[System.Serializable]
public class Save
{
    public string saveName;
    public float timeElapsed;
    public string timestamp;
    
    public int player1Exp;
    public int player2Exp;
    public int player1Lv;
    public int player2Lv;
    public int player1Hp;
    public int player2Hp;
    public int player1En;
    public int player2En;


    //PlayerPosition
    public Vector2 playerTransform;
    public int playerCurrentScene;
    public int playerDirection;
    public int PlayerState;

    

}

[System.Serializable]
public class Config
{
    public int gameLanguage;

    public int masterVolume;
    public int musicVolume;
    public int sfxVolume;

    public int screenResolution;
    public bool isFullscreen;
    public int targetFrameRate;

    public int textSpeed;
    public bool isRainDisabled;
    public int colorblindSetting;



}
