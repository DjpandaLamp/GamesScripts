using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UVScroller : MonoBehaviour
{
    public RawImage image;
    public float UVx;
    public float UVy;

    // Update is called once per frame
    void Update()
    {
        image.uvRect = new Rect(image.uvRect.x + (UVx*Time.deltaTime), image.uvRect.y + (UVy*Time.deltaTime), 1, 1);
    }
}
