using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextStartEnd : MonoBehaviour
{
    public TypeWriter TypeWriter;
    public TextFull TextFull;
    public Image face;
    public int width;
    public int index;

    public Sprite face00;
    public Sprite face01;
    public Sprite face02;
    
    public void CallTextWriter(int calledIndex)
    {
        index = calledIndex;
        TypeWriter.fullText = TextFull.strings[index];

            Debug.Log("face" + (TextFull.faces[index]).ToString()) ;
            face.sprite = GetFace(TextFull.faces[index]);
            TypeWriter.image.sprite = face.sprite;
        //= TextFull.GetFace(TextFull.faces[index]);
    }

    // Update is called once per frame
    void Update()
    {
        if (TypeWriter.isFinished && Input.GetMouseButton(0) == true || TypeWriter.isFinished && Input.GetKey("z") == true)
        {
            if (TextFull.strings[index + 1] != "0")
            {
                index += 1;
                face.sprite = GetFace(TextFull.faces[index]);
                TypeWriter.fullText = TextFull.strings[index];
            }
            else
            {

                gameObject.SetActive(false);

            }
        }
    }
    Sprite GetFace(float value)
    {
        switch (value)
        {
            case 0.1f:
                return face01;
            case 0.2f:
                return face02;
            default:
                return face00;
        }
    }
}

