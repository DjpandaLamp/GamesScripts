using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    public GameObject otherTeleporter;
    public Vector2 offset;
    public WeatherController weather;
    public Coroutine coroutine;

    private void Start()
    {
        weather = GameObject.Find("WeatherObject").GetComponent<WeatherController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(roomTeleport(collision));
            }
        }

        
    }
    IEnumerator roomTeleport(Collider2D collision)
    {
        //ParticleSystemRain.gameObject.SetActive(true);
        //ParticleSystemSplash.gameObject.SetActive(true);
        //ParticleSystemSnow.gameObject.SetActive(false);
        Color32 worldColor = weather.worldColor;
        //rainMain.startColor = new Color(1, 1, 1, 0.6f);
        //rainEmis.rateOverTime = 700;
      //  splashEmis.rateOverTime = 150;

        for (int i = 0; i < 250; i++)
        {
            weather.worldColor = Color.Lerp(weather.worldColor, Color.black, 0.03f);
            weather.sprite.color = weather.worldColor;
            yield return new WaitForEndOfFrame();
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = new Vector2(otherTeleporter.transform.position.x + offset.x, otherTeleporter.transform.position.y + offset.y);
        }

        for (int i = 0; i < 150; i++)
        {
            weather.worldColor = Color.Lerp(weather.worldColor, worldColor, 0.06f);
            weather.sprite.color = weather.worldColor;
            yield return new WaitForEndOfFrame();
        }
        coroutine = null;
        yield return null;
    }
}
