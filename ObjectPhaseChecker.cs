using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPhaseChecker : MonoBehaviour
{
    PlayerOverworldManager player;
    SpriteRenderer sprite;
    private void Start()
    {
        player = GameObject.Find("PlayerObject").GetComponent<PlayerOverworldManager>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (player.transform.position.y-0.5f > gameObject.transform.position.y-3)
        {
            sprite.sortingOrder = 2;   
        }
        else
        {
            sprite.sortingOrder = 0;
        }
    }
}
