using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{
    public TextMeshProUGUI textComp;
    public AudioSource audio;
    public float delay;
    public Image image;

    public string fullText;
    public string lastFrameFullText;
    public string currentText = "";
    public bool abortText = false;
    public bool isFinished = false;
    public bool allowSkipText = true;
    public bool hasSkipped = false;


    private void Awake()
    {
        lastFrameFullText = fullText;
        textComp = GetComponent<TextMeshProUGUI>();
        StartCoroutine(ShowText());
    }

    private void Update()
    {
        if (lastFrameFullText != fullText && isFinished != true)
        {
            abortText= true;
            
            StartCoroutine(ShowText());
        }
        if (lastFrameFullText != fullText)
        {
            StartCoroutine(ShowText());
        }

        lastFrameFullText = fullText;


    }


    IEnumerator ShowText()
    {
        if (abortText)
        {
            abortText = false;
            StopAllCoroutines();
            yield return null;
        }
        isFinished = false;
        for (int i = 0; i < fullText.Length+1; i++) 
        {
            if (Input.GetMouseButton(0) == true || Input.GetKey("z") == true)
            {
                if (allowSkipText == true && hasSkipped == false)
                {
                    i = fullText.Length;
                }
                hasSkipped = true;
            }
            else
            {
                hasSkipped = false;
            }
            currentText = fullText.Substring(0, i);
            audio.Play();
            if (i > 0)
            {
                char[] chars = currentText.ToCharArray();
                if (char.IsPunctuation(chars[i-1]))
                {
                    textComp.text = currentText;
                    if (chars[i-1] == char.Parse("."))
                    {
                        yield return new WaitForSeconds(delay * 8f);
                    }
                    else if (chars[i - 1] == char.Parse(","))
                    {
                        yield return new WaitForSeconds(delay * 4f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(delay);
                    }
                    
                }
                else
                {
                    textComp.text = currentText;
                    yield return new WaitForSeconds(delay);
                }
            }
            else
            {
                textComp.text = currentText;
                yield return new WaitForSeconds(delay);
            }
            
        }  
        isFinished= true;
    }
}
