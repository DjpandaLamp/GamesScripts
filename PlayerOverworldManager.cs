using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerOverworldManager : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody2D playerRigidbody;





    public float xVector;
    public float yVector;
    public float moveSpd;

    public float disSinceLastEncounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        GetInput();
        MovePlayer();
        

    }

    void GetInput()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            xVector = Mathf.Sign(Input.GetAxis("Horizontal"));
        }
        else
        {
            xVector = 0;
        }
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            yVector = Mathf.Sign(Input.GetAxis("Vertical"));
        }
        else
        {
            yVector = 0;
        }
    }

    void MovePlayer()
    {
        Vector2 movement = new Vector2(xVector, yVector);
        movement *= Time.deltaTime * moveSpd;
        disSinceLastEncounter += Vector2.Distance(Vector2.zero, movement)/15;

        //playerRigidbody.AddForce(movement);
        playerRigidbody.velocity = movement;
    }
    void EncounterCheck()
    {
        if (disSinceLastEncounter > 310)
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}
