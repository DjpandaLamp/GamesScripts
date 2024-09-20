using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyValueHolder : MonoBehaviour
{


    int fps;
    Vector2 res;
    bool full;
    bool vsync;
    float a1;
    float a2;
    float a3;

    public OverworldMenuManager OverworldMenuManager;
    public GameObject[] gameObjects;
    int[] fpsLookup;
    Vector2[] resLookup;


    private void Start()
    {
        fpsLookup = new int[14];
        fpsLookup[0] = 10;
        fpsLookup[1] = 15;
        fpsLookup[2] = 20;
        fpsLookup[3] = 30;
        fpsLookup[4] = 45;
        fpsLookup[5] = 60;
        fpsLookup[6] = 90;
        fpsLookup[7] = 120;
        fpsLookup[8] = 144;
        fpsLookup[9] = 160;
        fpsLookup[10] = 200;
        fpsLookup[11] = 240;
        fpsLookup[12] = 255;
        fpsLookup[13] = 0;


        resLookup = new Vector2[5];
        resLookup[0] = new Vector2(1280,720);
        resLookup[1] = new Vector2(1920, 1080);
        resLookup[2] = new Vector2(2560, 1440);
        resLookup[3] = new Vector2(3200, 1800);
        resLookup[4] = new Vector2(3840, 2160);


    }

    private void Update()
    {
        fps = fpsLookup[gameObjects[0].GetComponent<Dropdown>().value];
        res = resLookup[gameObjects[1].GetComponent<Dropdown>().value];
        full = gameObjects[2].GetComponent<Toggle>().isOn;
        vsync = gameObjects[3].GetComponent<Toggle>().isOn;

        a1 = gameObjects[4].GetComponent<Slider>().value;
        a2 = gameObjects[5].GetComponent<Slider>().value;
        a3 = gameObjects[6].GetComponent<Slider>().value;
    }
    void Apply()
    {
        OverworldMenuManager.ApplySetting(fps, res, full, vsync, a1, a2, a3);
    }

}
