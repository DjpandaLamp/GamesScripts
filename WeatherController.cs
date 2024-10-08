using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public Color worldColor;
    public SpriteRenderer sprite;
    public ParticleSystem ParticleSystemRain;
    public ParticleSystem ParticleSystemSplash;
    public int weather;

    private void Start()
    {
        ChangeWeather();
    }
    void ChangeWeather()
    {
        var rainMain = ParticleSystemRain.main;
        var rainEmis = ParticleSystemRain.emission;
        var splashMain = ParticleSystemSplash.emission;
        var splashEmis = ParticleSystemSplash.emission;

        switch (weather)
        {
            case 0: // Rain Regular
                worldColor = new Color32(100, 100, 100, 100);
                rainMain.startColor = new Color(1, 1, 1, 0.6f);
                rainEmis.rateOverTime = 700;
                splashEmis.rateOverTime = 150;
                break;
            case 1: //Rain Misty
                worldColor = new Color32(150, 150, 150, 70);
                break;
            case 2: //Rain Heavy
                worldColor = new Color32(70, 70, 70, 100);
                break;
            case 3: //Monsoon
                worldColor = new Color32(100, 100, 100, 100);
                splashEmis.rateOverTime = 150;
                break;

        }
        sprite.color = worldColor;
    }
}
