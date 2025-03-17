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

    public int[] partyIDs;

    public string[] partyname;
    public double[] healthPoints;
    public double[] energyPoints;
    public double[] attackPhysicalStat;
    public double[] defencePhysicalStat;
    public double[] attackEnergyStat;
    public double[] defenceEnergyStat;
    public double[] maxHealthPoints;
    public double[] maxEnergyPoints;
    public int[] levels;
    
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
        healthPoints = new double[4];
        energyPoints = new double[4];
        attackPhysicalStat = new double[4];
        defencePhysicalStat = new double[4];
        attackEnergyStat = new double[4];
        defenceEnergyStat = new double[4];
        maxHealthPoints = new double[4];
        maxEnergyPoints = new double[4];
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

            partyname[2] = data.agentName[3];
            healthPoints[2] = data.agentHPCurrent[3];
            energyPoints[2] = data.agentENCurrent[3];
            attackPhysicalStat[2] = data.agentATK[3];
            defencePhysicalStat[2] = data.agentDEF[3];
            attackEnergyStat[2] = data.agentEATK[3];
            defenceEnergyStat[2] = data.agentEDEF[3];
            maxHealthPoints[2] = data.agentHPMax[3];
            maxEnergyPoints[2] = data.agentENMax[3];
            levels[2] = data.agentLV[3];
        }



    }

    public void UpdateParty(int party1, int party2, int party3, int party4)
    {
        for (int i = 0; i < partyIDs.Length; i++)
        {
            if (party1 != 0)
            {
                partyIDs[i] = party1;
                party1 = 0;
            }
            else if (party2 != 0)
            {
                partyIDs[i] = party2;
                party2 = 0;
            }
            else if (party3 != 0)
            {
                partyIDs[i] = party3;
                party3 = 0;
            }
            else if (party4 != 0)
            {
                partyIDs[i] = party4;
                party4 = 0;
            }
            else
            {
                return;
            }
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

                    partyname[2] = data.agentName[3];
                    healthPoints[2] = data.agentHPCurrent[3];
                    energyPoints[2] = data.agentENCurrent[3];
                    attackPhysicalStat[2] = data.agentATK[3];
                    defencePhysicalStat[2] = data.agentDEF[3];
                    attackEnergyStat[2] = data.agentEATK[3];
                    defenceEnergyStat[2] = data.agentEDEF[3];
                    maxHealthPoints[2] = data.agentHPMax[3];
                    maxEnergyPoints[2] = data.agentENMax[3];
                    levels[2] = data.agentLV[3];
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

