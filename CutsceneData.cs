using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Scriptable Objects/CutsceneData")]
public class CutsceneData : ScriptableObject
{
    public Vector2[] cameraPositions;
    public bool[] cameraMoveTypes;

    public Vector3[] playerPositions;
    public bool[] playerMoveTypes;
    public bool[] playeranimation;


    public int[] textCallIndex;

}
