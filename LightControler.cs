using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControler : MonoBehaviour
{
    
    public GameObject maskObj;
    public GameObject middleLightObj;
    public GameObject topLightObj;
    public GameObject dampenerObj;

    public Color color;
    public SpriteRenderer sr_maskObj;
    public SpriteRenderer sr_middleLightObj;
    public SpriteRenderer sr_topLightObj;
    public SpriteRenderer sr_dampenerObj;

    public Vector2 basescale_maskObj;
    public Vector2 basescale_middleLightObj;
    public Vector2 basescale_topLightObj;
    public Vector2 basescale_dampenerObj;


    private void Start()
    {
        basescale_maskObj = new Vector2(maskObj.transform.localScale.x, maskObj.transform.localScale.y);
        basescale_dampenerObj = new Vector2(dampenerObj.transform.localScale.x, dampenerObj.transform.localScale.y);
        basescale_middleLightObj = new Vector2(middleLightObj.transform.localScale.x, middleLightObj.transform.localScale.y);
        basescale_topLightObj = new Vector2(topLightObj.transform.localScale.x, topLightObj.transform.localScale.y);

        sr_dampenerObj = dampenerObj.GetComponent<SpriteRenderer>();
        sr_maskObj = maskObj.GetComponent<SpriteRenderer>();
        sr_middleLightObj = middleLightObj.GetComponent<SpriteRenderer>();
        sr_topLightObj = topLightObj.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        sizeFlux(sr_topLightObj, topLightObj, basescale_topLightObj);
        sizeFlux(sr_dampenerObj, dampenerObj, basescale_dampenerObj);
        sizeFlux(sr_middleLightObj, middleLightObj, basescale_middleLightObj);
        sizeFlux(sr_maskObj, maskObj, basescale_maskObj);
    }

    void sizeFlux(SpriteRenderer sprite, GameObject gameObject, Vector2 scale)
    {
        sprite.color = new Color(color.r, color.g, color.b, sprite.color.a);
        gameObject.transform.localScale = scale + (scale * Mathf.PingPong(Time.time/10,0.2f)/2 + (scale * Mathf.PingPong(Time.time / 2, 0.05f))/6);
    }


}
