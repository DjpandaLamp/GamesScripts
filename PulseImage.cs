using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PulseImage : MonoBehaviour
{
    public Image image;
    public float AlphaTarget;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        image.color = ColorTarget();
    }

    Color ColorTarget()
    {
        AlphaTarget = Mathf.Clamp01(0.5f + (Mathf.Sin(Time.time*3)/2));
       // Color baseColor = new Color(1, 1, 1, 1);
        Color blend = new Color(1, 1, 1, AlphaTarget);
       // Color blendColor = (Color.Lerp(baseColor, blend, 0.5f + Mathf.PingPong(Time.time, 0.5f)));
        return blend;
    }
}
