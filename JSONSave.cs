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
    private ConfigScript configScript;
    public GameObject saveContainer;
    private string persistantPath;

    public int timeSincePreviousSave;

    public GameObject loadObjectPrefab;
    public GameObject[] loadObjects;
    

    public Config config;
    public Save Save;
    public AutoSave AutoSave;
    public Canvas canvas;

    private void Awake()
    {
        //LoadFromJSON(2,0);
        persistantPath = Application.persistentDataPath;
        Debug.Log(Application.persistentDataPath);
    }

    private void Start()
    { 


    }


    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
         if (scene.buildIndex != 0 && scene.buildIndex != 1 && scene.buildIndex != 2)
         {
              canvas = GameObject.FindWithTag("MainUI").GetComponent<Canvas>();
              

              PlayerOverworldManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOverworldManager>();
             
            menuManager = GameObject.Find("BaseMenuBlock").GetComponent<OverworldMenuManager>();
            persistantScript = GetComponent<GlobalPersistantScript>();
            configScript = GetComponent<ConfigScript>();
            loadObjects = new GameObject[new DirectoryInfo(persistantPath).GetFiles().Length - 1];
        }
    }



    private void Update()
    {
        while(PlayerOverworldManager == null && SceneManager.GetActiveScene().buildIndex >= 3)
        {
            PlayerOverworldManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOverworldManager>();
        }


        GetInput();
        if (menuManager != null)
        {
            if (!menuManager.isUp)
            {
                CleanSavedGames();
            }
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
            

       
            AutoSave.playerTransform = PlayerOverworldManager.transform.position;
            AutoSave.playerCurrentScene = SceneManager.GetActiveScene().buildIndex;



            string AUTOSaveData = JsonUtility.ToJson(AutoSave);
            string filePath = persistantPath + "/SaveDataAUTO.json";
            Debug.Log(filePath);
            System.IO.File.WriteAllText(filePath, AUTOSaveData);

        }
        if (type == 2) //Settings Autosave
        {
            config.masterVolume = configScript.audioMaster;
            config.musicVolume = configScript.audioMusic;
            config.sfxVolume = configScript.audioSFX;

            config.screenResolution = configScript.resolutionSetting;
            config.isFullscreen = configScript.fullScreenToggle;
            config.targetFrameRate = configScript.fpsSetting;
            config.isVsync = configScript.vSync;



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

            LoadSave(0);
            menuManager.isUp = false;
            menuManager.yPos = -500;
            Debug.Log("Successfully Loaded");
        }
        if (type == 1)
        {
            string filePath = persistantPath + "/SaveDataAUTO.json";
            string AUTOSaveData = System.IO.File.ReadAllText(filePath);
            AutoSave = JsonUtility.FromJson<AutoSave>(AUTOSaveData);

            LoadSave(1);
            menuManager.isUp = false;
            menuManager.yPos = -500;
            Debug.Log("Successfully Loaded");
        }
        if (type == 2)
        {
            string filePath = persistantPath + "/ConfigData.json";
            //if (System.IO.File.Exists(filePath))
            //{
                string configData = System.IO.File.ReadAllText(filePath);
                config = JsonUtility.FromJson<Config>(configData);
                LoadSave(2);
                Debug.Log("Successfully Loaded");
           // }
           // else
            //{
            //    Debug.Log("Setting Config File Does not Exist");
           // }
        }

    }

    public void LoadSave(int type)
    {
        if (type == 0)
        {
            
            SceneManager.LoadSceneAsync(Save.playerCurrentScene);
            StartCoroutine(SceneValueGrabber(type));
            persistantScript.globalTimeElapsed = Save.timeElapsed;
        }
        if (type == 1)
        {
            SceneManager.LoadSceneAsync(AutoSave.playerCurrentScene);
            StartCoroutine(SceneValueGrabber(type));
            
        }
        if (type == 2)
        {
            configScript.setSettings(config.targetFrameRate, config.screenResolution, config.isFullscreen, config.isVsync, config.masterVolume, config.musicVolume, config.sfxVolume);
        }
    }

    IEnumerator SceneValueGrabber(int type)
    {
        
        yield return new WaitForSeconds(0.05f);
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex != 0 && scene.buildIndex != 1 && scene.buildIndex != 2)
        {
           
          
            menuManager = GameObject.Find("BaseMenuBlock").GetComponent<OverworldMenuManager>();
          
            while(PlayerOverworldManager == null)
            {
                PlayerOverworldManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOverworldManager>();
            }
           
            persistantScript = GetComponent<GlobalPersistantScript>();
            configScript = GetComponent<ConfigScript>();
            loadObjects = new GameObject[new DirectoryInfo(persistantPath).GetFiles().Length - 1];
            if (type == 1)
            {
                PlayerOverworldManager.transform.position = new Vector3(AutoSave.playerTransform.x, AutoSave.playerTransform.y);
            }
            if (type == 0)
            {
                PlayerOverworldManager.transform.position = new Vector3(Save.playerTransform.x, Save.playerTransform.y);
            }

            //menuManager.mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            //canvas.worldCamera = menuManager.mainCamera;
        }
        yield break; 
    }

    public void LoadSavedGames()
    {
        if (menuManager != null)
        {
            if (saveContainer == null)
            {
                saveContainer = GameObject.FindWithTag("SaveContainer");
            } 
        }

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

    //Text Flags
    public bool textFlag0;
    public bool textFlag1;
    public bool textFlag2;
    public bool textFlag3;
    public bool textFlag4;
    public bool textFlag5;
    public bool textFlag6;
    public bool textFlag7;
    public bool textFlag8;
    public bool textFlag9;
    public bool textFlag10;
    public bool textFlag11;
    public bool textFlag12;

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
public class AutoSave
{
    public Vector2 playerTransform;
    public int playerCurrentScene;

}


[System.Serializable]
public class Config
{
    public int gameLanguage;

    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    public Vector2 screenResolution;
    public bool isFullscreen;
    public int targetFrameRate;
    public bool isVsync;



}
