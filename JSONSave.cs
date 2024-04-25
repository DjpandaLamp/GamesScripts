using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class JSONSave : MonoBehaviour
{
    private PlayerOverworldManager PlayerOverworldManager;
    private GlobalPersistantScript persistantScript;
    private OverworldMenuManager menuManager;
    private GameObject saveContainer;
    private string persistantPath;

    public int timeSincePreviousSave;

    public GameObject loadObjectPrefab;
    public GameObject[] loadObjects;

    public Config config;
    public Save Save;
    private void Start()
    {
        menuManager = GameObject.Find("BaseMenuBlock").GetComponent<OverworldMenuManager>();
        saveContainer = GameObject.Find("SaveContainer");
        persistantPath = Application.persistentDataPath;
        PlayerOverworldManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOverworldManager>();
        persistantScript = GetComponent<GlobalPersistantScript>();
        loadObjects = new GameObject[new DirectoryInfo(persistantPath).GetFiles().Length - 1];
    }
    private void Update()
    {
        GetInput();
        if (!menuManager.isUp)
        {
            CleanSavedGames();
        }
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveToJSON(2);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadFromJSON(2,0);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            DeleteAllSaves();
        }
    }




        public void SaveToJSON(int type)
    {
        if (type == 0) //Main Save
        {
            int num = new DirectoryInfo(persistantPath).GetFiles().Length - 1;

            Save.timestamp = System.DateTime.Now.ToString();

            //timeSincePreviousSave
            Save.timeElapsed = Mathf.Round(persistantScript.globalTimeElapsed);

            

            Save.playerTransform = PlayerOverworldManager.transform.position;
            Save.playerCurrentScene = SceneManager.GetActiveScene().buildIndex;



            string SaveData = JsonUtility.ToJson(Save);
            string filePath = persistantPath + "/SaveData" + num.ToString() + ".json";
            Debug.Log(filePath);
            System.IO.File.WriteAllText(filePath, SaveData);
            

            CleanSavedGames();
            LoadSavedGames();
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
    public void LoadFromJSON(int type, int num)
    {


        if(type == 0)
        {
            string filePath = persistantPath + "/SaveData" + num.ToString() + ".json";
            string SaveData = System.IO.File.ReadAllText(filePath);
            Save = JsonUtility.FromJson<Save>(SaveData);

            LoadSave();
            menuManager.isUp = false;
            menuManager.yPos = -500;
            Debug.Log("Successfully Loaded");
        }
        if (type == 1)
        {

        }
        if (type == 2)
        {
            string filePath = persistantPath + "/ConfigData.json";

            string configData = System.IO.File.ReadAllText(filePath);
            config = JsonUtility.FromJson<Config>(configData);

            Debug.Log("Successfully Loaded");
        }

    }

    public void LoadSave()
    {
        
        persistantScript.globalTimeElapsed = Save.timeElapsed;
        SceneManager.LoadSceneAsync(Save.playerCurrentScene);
        PlayerOverworldManager.transform.position = new Vector3(Save.playerTransform.x, Save.playerTransform.y);
        
        
    }

    public void LoadSavedGames()
    {
        if (Directory.Exists(Application.persistentDataPath))
        {
            

            DirectoryInfo d = new DirectoryInfo(persistantPath);
            for (int i = d.GetFiles().Length; i > 0; i--)
            {
                GameObject newsavebox = Instantiate(loadObjectPrefab, saveContainer.transform);
                string filePath = persistantPath + "/SaveData" + i.ToString() + ".json";
                if (System.IO.File.Exists(filePath))
                {
                    string SaveData = System.IO.File.ReadAllText(filePath);
                    Save = JsonUtility.FromJson<Save>(SaveData);
                    InternalSaveButtonScript ISBS = newsavebox.GetComponent<InternalSaveButtonScript>();
                    ISBS.saveID = i;
                    ISBS.saveNum.text = "Save#" + i.ToString();
                    ISBS.playtimeText.text = "Playtime" + Save.timeElapsed.ToString();
                    ISBS.timeStampText.text = "Date" + Save.timestamp.ToString();
                    ISBS.locationName.text = Save.playerCurrentScene.ToString();
                    loadObjects[i] = newsavebox;
                }
                else
                {
                    Destroy(newsavebox);
                }

            }
        }
        else
        {
            File.Create(Application.persistentDataPath);
            return;
        }
    }
    public void CleanSavedGames()
    {
        for (int i = 0; i < loadObjects.Length;i++)
        {
            Destroy(loadObjects[i]);
        }
        loadObjects = new GameObject[new DirectoryInfo(persistantPath).GetFiles().Length - 1];
    }


    void DeleteAllSaves()
    {
        DirectoryInfo d = new DirectoryInfo(persistantPath);
        for (int i = d.GetFiles().Length; i > 0; i--)
        {
            string filePath = persistantPath + "/SaveData" + i.ToString() + ".json";
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

        }
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
public class MenuSave
{
    public string saveName;
    public float timeElapsed;
    public string timestamp;
    public int playerCurrentScene;
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
