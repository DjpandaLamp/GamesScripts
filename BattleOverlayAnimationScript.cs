using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mathfunctions;
using UnityEngine.UI;

public class BattleOverlayAnimationScript : MonoBehaviour
{
    public GameObject screenOverlayObject;
    public RectTransform rect;
    public Image overlayImage;
    public GameObject uiBoxOverlayObject;
    public GameObject uiBoxPulseOverlayOject;
    public Vector2 position;
    


    public IEnumerator Animation(int type, int text)
    {
        rect = screenOverlayObject.GetComponent<RectTransform>();
        overlayImage = screenOverlayObject.GetComponent<Image>();
        if (type == 0) //Centre 
        {
            overlayImage.enabled = true;
            StartCoroutine(PrivMath.Vector2LerpRect(rect, new Vector2(0, 0), 0.1f, 200, 1));
            for (int i = 0; i < 200; i++) //fade in color
            {
                overlayImage.color = Color.Lerp(overlayImage.color, new Color(0, 0, 0, 0.8f), 0.05f);
                yield return new WaitForSeconds(0.01f);
            }
        }
        if (type == 1) //Move to corner
        {
            StartCoroutine(PrivMath.Vector2LerpRect(rect, new Vector2(-500, -275), 0.1f, 200, 1));
            for (int i = 0; i < 200; i++) //fade in color
            {
                overlayImage.color = Color.Lerp(overlayImage.color, new Color(0, 0, 0, 0f), 0.05f);
                yield return new WaitForSeconds(0.01f);
            }
            overlayImage.enabled = false;

        }




        yield return null;
    }
}
