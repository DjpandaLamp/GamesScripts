using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public Color worldColor;
    public SpriteRenderer sprite;
    public ParticleSystem ParticleSystemRain;
    public ParticleSystem ParticleSystemSplash;
    public ParticleSystem ParticleSystemSnow;
    public int weather;
    public float weatherTimer;
    public bool enableWeatherChange;

    private void Start()
    {
        ChangeWeather();
    }
    void ChangeWeather()
    {
        var rainMain = ParticleSystemRain.main;
        var SnowMain = ParticleSystemSnow.main;
        var rainEmis = ParticleSystemRain.emission;
        var splashMain = ParticleSystemSplash.emission;
        var splashEmis = ParticleSystemSplash.emission;

        switch (weather)
        {
            case 0: // Rain Regular
                ParticleSystemRain.gameObject.SetActive(true);
                ParticleSystemSplash.gameObject.SetActive(true);
                ParticleSystemSnow.gameObject.SetActive(false);
                worldColor = new Color32(100, 100, 100, 100);
                rainMain.startColor = new Color(1, 1, 1, 0.6f);
                rainEmis.rateOverTime = 700;
                splashEmis.rateOverTime = 150;
                break;
            case 1: //Rain Misty
                ParticleSystemRain.gameObject.SetActive(true);
                ParticleSystemSplash.gameObject.SetActive(true);
                ParticleSystemSnow.gameObject.SetActive(false);
                worldColor = new Color32(150, 150, 150, 70);
                rainMain.startColor = new Color(1, 1, 1, 0.6f);
                rainEmis.rateOverTime = 700;
                splashEmis.rateOverTime = 150;
                break;
            case 2: //Rain Heavy
                ParticleSystemRain.gameObject.SetActive(true);
                ParticleSystemSplash.gameObject.SetActive(true);
                ParticleSystemSnow.gameObject.SetActive(false);
                worldColor = new Color32(70, 70, 70, 100);
                rainMain.startColor = new Color(1, 1, 1, 0.5f);
                rainEmis.rateOverTime = 1400;
                splashEmis.rateOverTime = 200;
                break;
            case 3: //Monsoon
                ParticleSystemRain.gameObject.SetActive(true);
                ParticleSystemSplash.gameObject.SetActive(true);
                ParticleSystemSnow.gameObject.SetActive(false);
                worldColor = new Color32(100, 100, 100, 100);
                rainMain.startColor = new Color(1, 1, 1, 0.3f);
                rainEmis.rateOverTime = 3500;
                splashEmis.rateOverTime = 350;
                break;
            case 4: //Light Snow
                ParticleSystemRain.gameObject.SetActive(false);
                ParticleSystemSplash.gameObject.SetActive(false);
                ParticleSystemSnow.gameObject.SetActive(true);
                worldColor = new Color32(100, 100, 100, 100);

                break;
            case 5: //Snow
                ParticleSystemRain.gameObject.SetActive(false);
                ParticleSystemSplash.gameObject.SetActive(false);
                ParticleSystemSnow.gameObject.SetActive(true);
                worldColor = new Color32(100, 100, 100, 100);

                break;
            case 6: //Snow Storm
                ParticleSystemRain.gameObject.SetActive(false);
                ParticleSystemSplash.gameObject.SetActive(false);
                ParticleSystemSnow.gameObject.SetActive(true);
                worldColor = new Color32(100, 100, 100, 100);
                SnowMain.startColor = new Color(1, 1, 1, 1);
                break;
            case 7: //Clear
                ParticleSystemRain.gameObject.SetActive(false);
                ParticleSystemSplash.gameObject.SetActive(false);
                ParticleSystemSnow.gameObject.SetActive(false);
                worldColor = new Color32(100, 100, 100, 10);
                break;
            case 8: //Overcast
                ParticleSystemRain.gameObject.SetActive(false);
                ParticleSystemSplash.gameObject.SetActive(false);
                ParticleSystemSnow.gameObject.SetActive(false);
                worldColor = new Color32(100, 100, 100, 100);
                break;

        }
        sprite.color = worldColor;
        weatherTimer = 10;
    }
    private void FixedUpdate()
    {
        weatherTimer -= 1 * Time.deltaTime;
        if (weatherTimer < 0 & enableWeatherChange)
        {
            weather = Random.Range(0, 6);
            ChangeWeather();
        }
    }
}
