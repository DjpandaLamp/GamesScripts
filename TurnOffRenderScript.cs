using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurnOffRenderScript : MonoBehaviour
{
    public TilemapRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer.enabled = false;
    }

}
