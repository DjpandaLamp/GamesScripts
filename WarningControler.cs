using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningControler : MonoBehaviour
{
    public GameObject barTop;
    public GameObject barBottom;
    public GameObject steelTop;
    public GameObject steelBottom;
    public ParticleSystem ParticleSystem;
    public PlayerOverworldManager player;

    public bool isClosed;
    public bool abortClose;
    public int animationStep;
    

    private void Start()
    {
        player = GameObject.Find("PlayerObject").GetComponent<PlayerOverworldManager>();
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.Find("PlayerObject").GetComponent<PlayerOverworldManager>();
        }
        else
        {
            if (player.isChased)
            {
                if (isClosed)
                {
                    if (animationStep < 25)
                    {
                        animationStep++;
                    }
                    if (animationStep == 24)
                    {
                        ParticleSystem.Play();
                    }
                }
                else
                {
                    if (animationStep > 0)
                    {
                        animationStep--;
                    }
                    else if (animationStep < 0)
                    {
                        animationStep++;
                    }
                }
                barTop.GetComponent<RectTransform>().localPosition = new Vector2(0, Mathf.Clamp((180 - Mathf.Pow(1.23f, animationStep)), 12, 180) - Mathf.Clamp((animationStep) * 5, -50, 0));
                barBottom.GetComponent<RectTransform>().localPosition = new Vector2(0, Mathf.Clamp(-180 + (Mathf.Pow(1.23f, animationStep)), -180, -12) + Mathf.Clamp((animationStep) * 5, -50, 0));
                steelTop.GetComponent<RectTransform>().localPosition = new Vector2(0, Mathf.Clamp(300 - (Mathf.Pow(1.23f, animationStep)), 140, 300) - Mathf.Clamp((animationStep) * 5, -50, 0));
                steelBottom.GetComponent<RectTransform>().localPosition = new Vector2(0, Mathf.Clamp(-300 + (Mathf.Pow(1.23f, animationStep)), -300, -140) + Mathf.Clamp((animationStep) * 5, -50, 0));
            }
            else
            {
                {
                    if (animationStep > -10)
                    {
                        animationStep--;
                    }

                }
                barTop.GetComponent<RectTransform>().localPosition = new Vector2(0, Mathf.Clamp((180 - Mathf.Pow(1.23f, animationStep)), 12, 180) - Mathf.Clamp((animationStep) * 5, -50, 0));
                barBottom.GetComponent<RectTransform>().localPosition = new Vector2(0, Mathf.Clamp(-180 + (Mathf.Pow(1.23f, animationStep)), -180, -12) + Mathf.Clamp((animationStep) * 5, -50, 0));
                steelTop.GetComponent<RectTransform>().localPosition = new Vector2(0, Mathf.Clamp(300 - (Mathf.Pow(1.23f, animationStep)), 140, 300) - Mathf.Clamp((animationStep) * 5, -50, 0));
                steelBottom.GetComponent<RectTransform>().localPosition = new Vector2(0, Mathf.Clamp(-300 + (Mathf.Pow(1.23f, animationStep)), -300, -140) + Mathf.Clamp((animationStep) * 5, -50, 0));
            }
        }
    }
        
}

