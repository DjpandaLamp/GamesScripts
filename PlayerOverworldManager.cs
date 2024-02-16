using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverworldManager : MonoBehaviour
{
    public Transform playerTransform;





    public float xVector;
    public float yVector;

    public float moveSpd;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            xVector = Input.GetAxis("Horizontal") ;
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            yVector = Input.GetAxis("Vertical");
        }

        Vector2 movement = new Vector2(xVector, yVector);

        movement *= Time.deltaTime * moveSpd;

        playerTransform.Translate(movement);
        xVector = 0;
        yVector = 0;
    }
}
