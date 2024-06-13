using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliderDetectorScript : MonoBehaviour
{
    public EnemyOverworldManager enemy;

    private void Start()
    {
        enemy = GetComponentInParent<EnemyOverworldManager>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exit");
        if (collision.gameObject.tag == "Player" && gameObject.name == "PlayerDetectorOuter")
        {
            enemy.isChasing = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("enter");
        if (collision.gameObject.tag == "Player" && gameObject.name == "PlayerDetectorInner")
        {
            enemy.isChasing = true;
        }
    }
    
}
