using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigScript : MonoBehaviour
{
    public int fpsSetting = 60;
    public Vector2 resolutionSetting = new Vector2(1200,800);
    public bool fullScreenToggle = false;
    public bool vSync;

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
}
