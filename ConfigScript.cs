using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigScript : MonoBehaviour
{
    [Header("Graphics Settings")]
    public int fpsSetting = 60;
    public Vector2 resolutionSetting = new Vector2(1200,800);
    public bool fullScreenToggle = false;
    public bool vSync;
    [Header("Audio Settings")]
    public float audioMaster;
    public float audioMusic;
    public float audioSFX;



    private void Start()
    {

        Application.targetFrameRate = fpsSetting;
        Screen.SetResolution((int)resolutionSetting.x, (int)resolutionSetting.y, fullScreenToggle);
        if (vSync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }





    }
    public void setSettings(int fps, Vector2 res, bool fullscreen, bool vsync, float a1, float a2, float a3)
    {
        fpsSetting = fps;
        resolutionSetting = res;
        fullScreenToggle = fullscreen;
        vSync = vsync;
        audioMaster = a1;
        audioMusic = a2;
        audioSFX = a3;


        Application.targetFrameRate = fpsSetting;
        Screen.SetResolution((int)resolutionSetting.x, (int)resolutionSetting.y, fullScreenToggle);
        if (vSync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

    }

    public void Update()
    {
        Application.targetFrameRate = fpsSetting;
    }

}
