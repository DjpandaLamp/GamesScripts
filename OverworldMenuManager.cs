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
    public bool isUp;
    public Transform selfTransform;
    public GameObject[] menuObjects;

    public StateMachine State;
    
    // Start is called before the first frame update
    void Start()
    {
        selfTransform = GetComponent<RectTransform>();
        StateSetter(0);
    }

    // Update is called once per frame
    void Update()
    {
        UIUpChecker();
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
                if (yPos < -440)
                {
                    StateSetter(0);
                }
            }
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                isUp = false;
            }
            
        }
        if (isUp)
        {
            if (yPos < 0)
            {
                yPos += 2500 * Time.deltaTime;
                if (yPos > 0)
                {
                    yPos = 0;
                }
            }


            transform.localPosition = new Vector3(0, yPos, 0);
        }
        else
        {
            if (yPos > -500)
            {
                yPos -= 2500 * Time.deltaTime;
                if (yPos < -500)
                {
                    yPos = -500;
                }
            }

            transform.localPosition = new Vector3(0, yPos, 0);
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
}
