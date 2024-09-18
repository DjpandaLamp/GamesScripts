using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextStartEnd : MonoBehaviour
{
    public TypeWriter TypeWriter;
    public TextFull TextFull;
    private Image face;
    public int width;

    public int index;
    // Start is called before the first frame update
    void Start()
    {
        face = GetComponentInChildren<Image>();
    }

    public void CallTextWriter(int calledIndex)
    {
        index = calledIndex;
        if (TextFull.faces[calledIndex] != Vector2.zero)
        {
            Debug.Log("face" + (((int)TextFull.faces[calledIndex].x * width) + (int)TextFull.faces[calledIndex].y).ToString());
            face.sprite = TextFull.sprite[((int)TextFull.faces[calledIndex].x * width) + (int)TextFull.faces[calledIndex].y];
            TypeWriter.image.sprite = face.sprite;
           
        }
        TypeWriter.fullText = TextFull.strings[index];
    }

    // Update is called once per frame
    void Update()
    {
        if (TypeWriter.isFinished && Input.GetMouseButton(0) == true || TypeWriter.isFinished && Input.GetKey("z") == true)
        {
            if (TextFull.strings[index + 1] != "0")
            {
                index += 1;
                TypeWriter.fullText = TextFull.strings[index];
            }
            else
            {

                gameObject.SetActive(false);

            }
        }
    }
}
