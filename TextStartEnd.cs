using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextStartEnd : MonoBehaviour
{
    public TypeWriter TypeWriter;
    public TextFull TextFull;

    public int index;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CallTextWriter(int calledIndex)
    {
        index = calledIndex;
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
