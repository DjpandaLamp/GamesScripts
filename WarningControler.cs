using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningControler : MonoBehaviour
{
    public GameObject barTop;
    public GameObject barBottom;
    public GameObject barLeft;
    public GameObject barRight;
    public GameObject steelTop;
    public GameObject steelBottom;
    public GameObject steelRight;
    public GameObject steelLeft;

    public bool isClosed;
    public bool abortClose;

    private void Update()
    {
        if (isClosed)
        {
            barTop.GetComponent<RectTransform>().localPosition = new Vector2(0, 12.5f);
            barBottom.GetComponent<RectTransform>().localPosition = new Vector2(0, -12.5f);
            //barLeft.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            //barRight.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            steelTop.GetComponent<RectTransform>().localPosition = new Vector2(0, 140);
            steelBottom.GetComponent<RectTransform>().localPosition = new Vector2(0, -140);
           // steelRight.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        }
    }


}
