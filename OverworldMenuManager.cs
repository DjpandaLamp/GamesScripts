using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldMenuManager : MonoBehaviour
{
    public enum StateMachine
    {
        Base,
        Inventory,
        Equiment,
        PartyConfig,
        Map,
        Settings,
        Save,
        Load,
        Quit,
        Empty
    }

    public float yPos = 0;
    public float yPosSet = -500;
    public float cameraXOffset = 0;
    public float cameraXOffsetSet = -5;


    public bool isUp;
    
    public Transform selfTransform;
    public GameObject[] menuObjects;
    public GameObject[] settingsObjects;
    public TextStartEnd textSE;
    public GameObject perst;
    public GlobalPersistantScript GlobalPersistant;
    public ConfigScript config;

    public JSONSave JSONSave;

    public Camera mainCamera;

    public StateMachine State;

    public InventoryItemManager inventory;
    public GameObject inventoryContainer;
    public GameObject inventoryButtonPrefab;
    public List<GameObject> inventoryObjects;

    
    // Start is called before the first frame update
    void Start()
    {
        textSE = GameObject.FindObjectOfType<TextStartEnd>(true);
        selfTransform = GetComponent<RectTransform>();
        perst = GameObject.FindWithTag("Persistant");
        GlobalPersistant = perst.GetComponent<GlobalPersistantScript>();
        config = perst.GetComponent<ConfigScript>();
        JSONSave = perst.GetComponent<JSONSave>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        inventory = perst.GetComponent<InventoryItemManager>();
        StateSetter(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1 && SceneManager.GetActiveScene().buildIndex != 2)
        {
            UIUpChecker();
        }
        if (textSE.isActiveAndEnabled == true || isUp)
        {
            GlobalPersistant.isPaused = true;
        }
        else
        {
            GlobalPersistant.isPaused = false;
        }
        
    }

    void UIUpChecker()
    {
        if (Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.I))
        {

            if (isUp)
            {
                isUp = !isUp;
            }
            else
            {
                isUp = !isUp;
                if (yPos < yPosSet+60)
                {
                    StateSetter(0);
                }
            }
            if (textSE.isActiveAndEnabled == true)
            {
                isUp = false;
            }
            
        }
        if (isUp)
        {
            if (yPos < 0)
            {
                yPos += 2000 * Time.deltaTime;
                if (yPos > 0)
                {
                    yPos = 0;
                }
            }

            if (cameraXOffset > cameraXOffsetSet)
            {
                cameraXOffset -= 20 * Time.deltaTime;
                if (cameraXOffset < cameraXOffsetSet)
                {
                    cameraXOffset = cameraXOffsetSet;
                }
            }
            mainCamera.transform.localPosition = new Vector3(cameraXOffset, 0, -45);
            transform.localPosition = new Vector3(-180, yPos, 0);
        }
        else
        {
            if (yPos > yPosSet)
            {
                yPos -= 2000 * Time.deltaTime;
                if (yPos < yPosSet)
                {
                    yPos = yPosSet;
                }
            }
            if (cameraXOffset < 0)
            {
                cameraXOffset += 20 * Time.deltaTime;
                if (cameraXOffset > 0)
                {
                    cameraXOffset = 0;
                }
            }
            mainCamera.transform.localPosition = new Vector3(cameraXOffset, 0, -45);
            transform.localPosition = new Vector3(-180, yPos, 0);
        }
    }

    public void StateSetter(int state)
    {
        switch(state)
        {
            case 0:
                State = StateMachine.Base;
                break;
            case 1:
                State = StateMachine.Inventory;
                break;
            case 2:
                State = StateMachine.Equiment;
                break;
            case 3:
                State = StateMachine.PartyConfig;
                break;
            case 4:
                State = StateMachine.Map;
                break;
            case 5:
                State = StateMachine.Save;
                break;
            case 6:
                State = StateMachine.Load;
                break;
            case 7:
                State = StateMachine.Quit;
                break;
            case 8:
                State = StateMachine.Settings;
                break;
            case 9:
                State = StateMachine.Empty;
                break;
        }

        if (State == StateMachine.Base)
        {
            MenuReseter();
            menuObjects[0].SetActive(true);
        }
        if (State == StateMachine.Inventory)
        {
            MenuReseter();
            menuObjects[1].SetActive(true);
            resetInventory(ItemTag.ConsumableHeal);
        }
        if (State == StateMachine.Equiment)
        {
            MenuReseter();
            menuObjects[2].SetActive(true);
        }
        if (State == StateMachine.PartyConfig)
        {
            MenuReseter();
            menuObjects[3].SetActive(true);
        }
        if (State == StateMachine.Map)
        {
            MenuReseter();
            menuObjects[4].SetActive(true);
        }
        if (State == StateMachine.Save)
        {
            MenuReseter();
            menuObjects[5].SetActive(true);
            JSONSave.LoadSavedGames();
        }
        if (State == StateMachine.Load)
        {
            MenuReseter();
            menuObjects[6].SetActive(true);
        }
        if (State == StateMachine.Quit)
        {
            MenuReseter();
            menuObjects[7].SetActive(true);
        }
        if (State == StateMachine.Settings)
        {
            MenuReseter();
            menuObjects[8].SetActive(true);
        }
        if (State == StateMachine.Empty)
        {
            MenuReseter();
        }

    }

    void MenuReseter()
    {
        for (int i = 0; i < menuObjects.Length; i++)
        {
            menuObjects[i].SetActive(false);
        }
    }
    public void ApplySetting(int fps, Vector2 res, bool fullscreen, bool vsync, float a1, float a2, float a3)
    {
        config.setSettings(fps,res,fullscreen,vsync,a1,a2,a3);
        JSONSave.SaveToJSON(2);
    }

    #region

    public void resetInventory(ItemTag filter)
    {
        for (int i = 0; i < inventoryObjects.ToArray().Length; i++)
        {
            Destroy(inventoryObjects[i]);
        }
        if (inventoryObjects.ToArray().Length > 0)
        {
            inventoryObjects.Clear();
        }


        for (int i = 0; i < inventory.itemCounts.Length; i++)
        {
            if (inventory.itemCounts[i] > 0 && inventory.items[i].ItemTag == filter)
            {
                GameObject newer = Instantiate(inventoryButtonPrefab, inventoryContainer.transform);
                inventoryObjects.Add(newer);
            }
        }
    }


    #endregion

}
