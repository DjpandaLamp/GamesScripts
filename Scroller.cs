using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    // Start is called before the first frame update
    
    
    public float sinX;
    public float sinY;

    public float time;
    public float timeOffset;
    public float rot;
    public float rotOffset;
    public float speedMult;
   

    public Vector2 realpos;

    public RectTransform rect;

    void Start()
    {
       // rotOffset = 0;
        //speedMult = 1;
        //rotOffset = Random.Range(0, 360);
        speedMult = Random.Range(0.8f, 1.2f);
        
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        time += .5f * Time.deltaTime;
        time = time - timeOffset;
        rot += 1f * Time.deltaTime;
        sinX = Mathf.Sin(time*speedMult);
        sinY = Mathf.Sin(time*speedMult);

        sinX = sinX * Mathf.Cos(rot + rotOffset) - sinY * Mathf.Sin(rot + rotOffset) ;
        sinY = sinX * Mathf.Sin(rot + rotOffset) + sinY * Mathf.Cos(rot + rotOffset);

        realpos = new Vector2(sinX, sinY);
        rect.position = realpos;

    }
}
