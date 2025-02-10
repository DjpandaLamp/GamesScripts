using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


namespace Mathfunctions
{
    public class PrivMath : MonoBehaviour
    {

        public static IEnumerator Vector2LerpRect(RectTransform rect, Vector2 b, float t, int i, int type)
        {
            if (type == 0)
            {
                Vector2 a = rect.sizeDelta;
                for (int j = 0; j < i; j++)
                {
                    a = Vector2.Lerp(a, b, t);
                    rect.sizeDelta = a;
                    yield return new WaitForSeconds(0.01f);
                }
            }
            if (type == 1)
            {
                Vector2 a = rect.localPosition;
                for (int j = 0; j < i; j++)
                {
                    a = Vector2.Lerp(a, b, t);
                    rect.localPosition = a;
                    yield return new WaitForSeconds(0.01f);
                }
            }
            yield return null;
        }
    }
}