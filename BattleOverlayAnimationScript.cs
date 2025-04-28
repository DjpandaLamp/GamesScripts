using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mathfunctions;
using UnityEngine.UI;
using TMPro;

public class BattleOverlayAnimationScript : MonoBehaviour
{
    public GameObject screenOverlayObject;
    public RectTransform rect;
    public Image overlayImage;
    public GameObject uiBoxOverlayObject;
    public GameObject uiBoxPulseOverlayOject;

    public TextMeshProUGUI textBox1;
    public TextMeshProUGUI textBox1_Pulse;
    public TextMeshProUGUI textBox2;

    public Vector2 position;
    


    public IEnumerator Animation(int type, int text)
    {
        rect = screenOverlayObject.GetComponent<RectTransform>();
        overlayImage = screenOverlayObject.GetComponent<Image>();
        if (type == 0) //Centre 
        {
            overlayImage.enabled = true;
            StartCoroutine(PrivMath.Vector2LerpRect(rect, new Vector2(0, 0), 0.1f, 100, 1));
            for (int i = 0; i < 200; i++) //fade in color
            {
                overlayImage.color = Color.Lerp(overlayImage.color, new Color(0, 0, 0, 0.8f), 0.05f);
                screenOverlayObject.transform.localScale = Vector2.Lerp(screenOverlayObject.transform.localScale, new Vector2(1f, 1f), 0.05f);
                yield return new WaitForSeconds(0.01f);
            }
        }
        if (type == 1) //Move to corner
        {
            StartCoroutine(PrivMath.Vector2LerpRect(rect, new Vector2(-250, 275), 0.1f, 100, 1));
            for (int i = 0; i < 100; i++) //fade in color
            {
                overlayImage.color = Color.Lerp(overlayImage.color, new Color(0, 0, 0, 0f), 0.1f);
                screenOverlayObject.transform.localScale = Vector2.Lerp(screenOverlayObject.transform.localScale, new Vector2(1f, 1f), 0.1f);
                yield return new WaitForSeconds(0.01f);
            }
            overlayImage.enabled = false;

        }
        if (type == 2) // 
        {
            Color _c = textBox1.color;
            for (int i = 0; i < 75; i++) //fade in color
            {
                textBox1.color = Color.Lerp(textBox1.color, new Color32(255, 255, 255, 255), 0.05f);
                yield return new WaitForSeconds(0.01f);
            }
            textBox1.text = "PREP TURN";
            textBox1_Pulse.text = textBox1.text;
            textBox1.color = _c;
            for (int i = 0; i < 75; i++) //fade in color
            {
                textBox1_Pulse.transform.localScale = Vector2.Lerp(textBox1_Pulse.transform.localScale, new Vector2(1.5f,1.5f), 0.1f);
                textBox1_Pulse.color = Color.Lerp(textBox1_Pulse.color, new Color32(255, 255, 255, 0), 0.05f);
                yield return new WaitForSeconds(0.01f);
            }
            textBox1_Pulse.text = "";
            textBox1_Pulse.transform.localScale = new Vector2(1f, 1f);
            textBox1_Pulse.color = new Color32(255, 255, 255, 255);
        }

        if (type == 3) // 
        {
            Color _c = textBox1.color;
            for (int i = 0; i < 75; i++) //fade in color
            {
                textBox1.color = Color.Lerp(textBox1.color, new Color32(255, 255, 255, 255), 0.05f);
                yield return new WaitForSeconds(0.01f);
            }
            textBox1.text = "BATTLE TURN";
            textBox1_Pulse.text = textBox1.text;
            textBox1.color = _c;
            for (int i = 0; i < 75; i++) //fade in color
            {
                textBox1_Pulse.transform.localScale = Vector2.Lerp(textBox1_Pulse.transform.localScale, new Vector2(1.5f, 1.5f), 0.1f);
                textBox1_Pulse.color = Color.Lerp(textBox1_Pulse.color, new Color32(255, 255, 255, 0), 0.05f);
                yield return new WaitForSeconds(0.01f);
            }
            textBox1_Pulse.text = "";
            textBox1_Pulse.transform.localScale = new Vector2(1f, 1f);
            textBox1_Pulse.color = new Color32(255, 255, 255, 255);
        }
    



        yield return null;
    }
}
