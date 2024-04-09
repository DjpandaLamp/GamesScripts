using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMenuManager : MonoBehaviour
{
    public float yPos = 0;
    public bool isUp;
    public Transform selfTransform;
    
    
    // Start is called before the first frame update
    void Start()
    {
        selfTransform = GetComponent<RectTransform>();
      
    }

    // Update is called once per frame
    void Update()
    {
        UIUpChecker();
    }

    void UIUpChecker()
    {
      
        if (Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.I))
        {
            isUp = !isUp;
        }
        if (isUp)
        {
            if (yPos < 0)
            {
                yPos += 2500 * Time.deltaTime;
                if (yPos > 0)
                {
                    yPos = 0;
                }
            }


            transform.localPosition = new Vector3(transform.position.x, yPos, transform.position.z);
        }
        else
        {
            if (yPos > -500)
            {
                yPos -= 2500 * Time.deltaTime;
                if (yPos < -500)
                {
                    yPos = -500;
                }
            }

            transform.localPosition = new Vector3(transform.position.x, yPos, transform.position.z);
        }
    }
}
