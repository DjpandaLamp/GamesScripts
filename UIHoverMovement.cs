using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHoverMovement : MonoBehaviour
{
    public Vector2 startPos;
    public float yPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        yPos = Mathf.Sin(Time.time*2)/6;
        gameObject.transform.position = new Vector3(startPos.x, startPos.y+yPos, -1);
    }
}
