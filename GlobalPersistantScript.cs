using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalPersistantScript : MonoBehaviour
{
    public Transform playerSavedTransform;
    public EnemyData data;
    public GameObject overworldMenu;
    public GameObject cameraPrefab;


    public string[] partyname;
    public int[] healthPoints;
    public int[] energyPoints;
    public int[] attackPhysicalStat;
    public int[] defencePhysicalStat;
    public int[] attackEnergyStat;
    public int[] defenceEnergyStat;
    public int[] maxHealthPoints;
    public int[] maxEnergyPoints;
    public int[] levels;
    /*
    public string p1NM;
    public int p1HP;
    public int p1EN;
    public int p1AT;
    public int p1DF;
    public int p1EAT;
    public int p1EDF;
    public int p1MHP;
    public int p1MEN;
    public int p1LV;

    public string p2NM;
    public int p2HP;
    public int p2EN;
    public int p2AT;
    public int p2DF;
    public int p2EAT;
    public int p2EDF;
    public int p2MHP;
    public int p2MEN;
    public int p2LV;
    */
    public int[] agentLevelPoint;

    public bool firstTimeLoad; //if false, load save data

    public float globalTimeElapsed;
    public float worldtime;
    public bool isPaused;


    [HideInInspector] public bool textFlag0;
    [HideInInspector] public bool textFlag1;
    [HideInInspector] public bool textFlag2;
    [HideInInspector] public bool textFlag3;
    [HideInInspector] public bool textFlag4;
    [HideInInspector] public bool textFlag5;
    [HideInInspector] public bool textFlag6;
    [HideInInspector] public bool textFlag7;
    [HideInInspector] public bool textFlag8;
    [HideInInspector] public bool textFlag9;
    [HideInInspector] public bool textFlag10;
    [HideInInspector] public bool textFlag11;
    [HideInInspector] public bool textFlag12;

    // Start is called before the first frame update
    void Start()
    {
        partyname = new string[4];
        healthPoints = new int[4];
        energyPoints = new int[4];
        attackPhysicalStat = new int[4];
        defencePhysicalStat = new int[4];
        attackEnergyStat = new int[4];
        defenceEnergyStat = new int[4];
        maxHealthPoints = new int[4];
        maxEnergyPoints = new int[4];
        levels = new int[4];

        if (overworldMenu == null & GameObject.FindWithTag("MainUI") != null)
        {
            overworldMenu = GameObject.FindWithTag("MainUI");
        }
        if (overworldMenu != null)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 0)
            {
                overworldMenu.SetActive(false);
            }
            else
            {
                overworldMenu.SetActive(true);
            }
        }

        
        if (data != null)
        {
            partyname[0] = data.agentName[1];
            healthPoints[0] = data.agentHPCurrent[1];
            energyPoints[0] = data.agentENCurrent[1];
            attackPhysicalStat[0] = data.agentATK[1];
            defencePhysicalStat[0] = data.agentDEF[1];
            attackEnergyStat[0] = data.agentEATK[1];
            defenceEnergyStat[0] = data.agentEDEF[1];
            maxHealthPoints[0] = data.agentHPMax[1];
            maxEnergyPoints[0] = data.agentENMax[1];
            levels[0] = data.agentLV[1];

            partyname[1] = data.agentName[2];
            healthPoints[1] = data.agentHPCurrent[2];
            energyPoints[1] = data.agentENCurrent[2];
            attackPhysicalStat[1] = data.agentATK[2];
            defencePhysicalStat[1] = data.agentDEF[2];
            attackEnergyStat[1] = data.agentEATK[2];
            defenceEnergyStat[1] = data.agentEDEF[2];
            maxHealthPoints[1] = data.agentHPMax[2];
            maxEnergyPoints[1] = data.agentENMax[2];
            levels[1] = data.agentLV[2];
        }



    }

    private void Update()
    {
        globalTimeElapsed += Time.deltaTime;
        worldtime += Time.deltaTime;
        if (overworldMenu != null)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 2)
            {
                overworldMenu.SetActive(false);
            }
            else
            {
                overworldMenu.SetActive(true);
            }
        }
        else
        {
            if (GameObject.FindWithTag("OverMenu") != null)
            {
                overworldMenu = GameObject.FindWithTag("OverMenu");
            }

            if (data == null)
            {
                if (SceneManager.GetActiveScene().buildIndex != 1 && SceneManager.GetActiveScene().buildIndex != 0)
                {
                    data = GameObject.Find("LevelData").GetComponent<EnemyData>();
                    partyname[0] = data.agentName[1];
                    healthPoints[0] = data.agentHPCurrent[1];
                    energyPoints[0] = data.agentENCurrent[1];
                    attackPhysicalStat[0] = data.agentATK[1];
                    defencePhysicalStat[0] = data.agentDEF[1];
                    attackEnergyStat[0] = data.agentEATK[1];
                    defenceEnergyStat[0] = data.agentEDEF[1];
                    maxHealthPoints[0] = data.agentHPMax[1];
                    maxEnergyPoints[0] = data.agentENMax[1];
                    levels[0] = data.agentLV[1];

                    partyname[1] = data.agentName[2];
                    healthPoints[1] = data.agentHPCurrent[2];
                    energyPoints[1] = data.agentENCurrent[2];
                    attackPhysicalStat[1] = data.agentATK[2];
                    defencePhysicalStat[1] = data.agentDEF[2];
                    attackEnergyStat[1] = data.agentEATK[2];
                    defenceEnergyStat[1] = data.agentEDEF[2];
                    maxHealthPoints[1] = data.agentHPMax[2];
                    maxEnergyPoints[1] = data.agentENMax[2];
                    levels[1] = data.agentLV[2];
                }

            }
        }

    }

    // Update is called once per frame
    public void OverworldUIChecker()
    {
        if (overworldMenu == null & GameObject.FindWithTag("MainUI") != null)
        {
            overworldMenu = GameObject.FindWithTag("MainUI");
        }
    }
}

