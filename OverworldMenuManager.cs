using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

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
    public float cameradepth = 5;
    public float cameradepthSet = 5;


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
    private PostProcessVolume post_c;
    private DepthOfField depth;
    private ColorGrading ColorGrading;

    public StateMachine State;

    [Header("Inventory")]
    public InventoryItemManager inventory;
    public GameObject inventoryContainer;
    public GameObject inventoryButtonPrefab;
    public List<GameObject> inventoryObjects;
    public Image inventoryImage;
    public Sprite[] inventoryImageArray;
    public TMP_Text inventoryObjectText;
    public TMP_Text inventoryEffectText;
    public TMP_Text inventoryDescText;

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
        post_c = GameObject.FindWithTag("MainCamera").GetComponent<PostProcessVolume>();
        post_c.profile.TryGetSettings(out depth);
        post_c.profile.TryGetSettings(out ColorGrading);

        inventory = perst.GetComponent<InventoryItemManager>();
        StateSetter(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            post_c = GameObject.FindWithTag("MainCamera").GetComponent<PostProcessVolume>();
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
            if (cameradepth > 0)
            {
                cameradepth -= 15 * Time.deltaTime;
                if (cameradepth < 0.1f)
                {
                    cameradepth = 0.1f;
                }
            }


            //mainCamera.transform.localPosition = new Vector3(cameradepth, 0, -45);
            ColorGrading.colorFilter.value = Color.Lerp(ColorGrading.colorFilter, new Color32(150, 20, 100, 1), 0.02f);
            depth.focusDistance.value = cameradepth;
            transform.localPosition = new Vector3(-125, yPos, 0);
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
            if (cameradepth < cameradepthSet)
            {
                cameradepth += 15 * Time.deltaTime;
                if (cameradepth > cameradepthSet)
                {
                    cameradepth = cameradepthSet;
                }
            }
            //mainCamera.transform.localPosition = new Vector3(cameradepth, 0, -45);
            ColorGrading.colorFilter.value = Color.Lerp(ColorGrading.colorFilter, Color.white, 0.02f);
            depth.focusDistance.value = cameradepth;
            transform.localPosition = new Vector3(-125, yPos, 0);
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
            resetInventory(TagGroups.Consumable);
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

    public void inventorySorterOnClick(int i)
    {
        if (i == 0)
        {
            resetInventory(TagGroups.Armor);
        }
        if (i == 1)
        {
            resetInventory(TagGroups.Consumable);
        }
        if (i == 2)
        {
            resetInventory(TagGroups.Weapon);
        }
    }


    public void resetInventory(TagGroups filter)
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
            if (inventory.itemCounts[i] > 0 && inventory.items[i].tagGroup == filter)
            {
                GameObject newer = Instantiate(inventoryButtonPrefab, inventoryContainer.transform);
                InventoryButtonData data = newer.GetComponent<InventoryButtonData>();
                data.ID = i;
                data.textCount.text = inventory.itemCounts[i].ToString() + "/99";
                data.textName.text = inventory.items[i].displayName;
                switch(inventory.items[i].ItemTag)
                {
                    case ItemTag.ArmorHead:
                        {
                            data.textCategory.text = "Head";
                            break;
                        }
                    case ItemTag.ArmorGloves:
                        {
                            data.textCategory.text = "Gloves";
                            break;
                        }
                    case ItemTag.ArmorHelmet:
                        {
                            data.textCategory.text = "Helmet";
                            break;
                        }
                    case ItemTag.ArmorPants:
                        {
                            data.textCategory.text = "Pants";
                            break;
                        }
                    case ItemTag.ArmorRing:
                        {
                            data.textCategory.text = "Ring";
                            break;
                        }
                    case ItemTag.ArmorShirt:
                        {
                            data.textCategory.text = "Shirt";
                            break;
                        }
                    case ItemTag.ArmorShoes:
                        {
                            data.textCategory.text = "Shoe";
                            break;
                        }
                    case ItemTag.ArmorSocks:
                        {
                            data.textCategory.text = "Socks";
                            break;
                        }
                    case ItemTag.ConsumableHeal:
                        {
                            data.textCategory.text = "Heal";
                            break;
                        }
                    case ItemTag.ConsumableStatus:
                        {
                            data.textCategory.text = "Cure";
                            break;
                        }
                }
                switch (inventory.items[i].ItemTag)
                {
                    case ItemTag.ConsumableHeal:
                        {
                            data.textValue.text = "Health+ " + inventory.items[i].healthValue.ToString();
                            data.textValue.color = Color.green;
                            break;
                        }
                    case ItemTag.ConsumableStatus:
                        {
                            data.textValue.text = "Cures: " + inventory.items[i].status.ToString();
                            data.textValue.color = Color.magenta;
                            break;
                        }
                    case ItemTag.Weapon:
                        {
                            data.textValue.text = "Damage: " + inventory.items[i].ArmorValue.ToString();
                            data.textValue.color = Color.red;
                            break;
                        }
                    default:
                        {
                            data.textValue.text = "Armor+ " + inventory.items[i].ArmorValue.ToString();
                            data.textValue.color = Color.grey;
                            break;
                        }
                }
                data.textCount.text = inventory.itemCounts[i].ToString() + "/99";
                inventoryObjects.Add(newer);
            }
        }
    }
    public void SetDescription(int index)
    {
        inventoryObjectText.text = inventory.items[index].displayName;
        inventoryDescText.text = inventory.items[index].desc;
        //inventoryEffectText.text = ;
        switch (inventory.items[index].ItemTag)
        {
            case ItemTag.ConsumableHeal:
                {
                    inventoryEffectText.text = "Heals: + " + inventory.items[index].healthValue.ToString();
                    inventoryEffectText.color = Color.green;
                    inventoryImage.sprite = inventoryImageArray[2];
                    break;
                }
            case ItemTag.ConsumableStatus:
                {
                    inventoryEffectText.text = "Cures: " + inventory.items[index].status.ToString();
                    inventoryEffectText.color = Color.magenta;
                    inventoryImage.sprite = inventoryImageArray[1];
                    break;
                }
            case ItemTag.Weapon:
                {
                    inventoryEffectText.text = "Damage: +" + inventory.items[index].ArmorValue.ToString();
                    inventoryEffectText.color = Color.red;
                    inventoryImage.sprite = inventoryImageArray[0];
                    break;
                }
            default:
                {
                    inventoryEffectText.text = "Armor: +" + inventory.items[index].ArmorValue.ToString();
                    inventoryEffectText.color = Color.grey;
                    inventoryImage.sprite = inventoryImageArray[0];
                    break;
                }
        }

    
    }
}
