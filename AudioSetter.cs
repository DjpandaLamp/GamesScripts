using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSetter : MonoBehaviour
{
    private ConfigScript config;
    public int audioType;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        if (config == null)
        {
            config = GameObject.FindWithTag("Persistant").GetComponent<ConfigScript>();
        }
        //audioSource.volume = config.audioMaster;
        if (audioType == 0)
        {
            //audioSource.volume *= config.audioMusic;
        }
        if (audioType == 1)
        {
           // audioSource.volume *= config.audioSFX;
        }
    }
}
