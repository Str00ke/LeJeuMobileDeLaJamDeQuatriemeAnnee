using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenBallsMatSprites))]
public class GenBallsMatSpritesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GenBallsMatSprites myScript = (GenBallsMatSprites)target;
        if (GUILayout.Button("Generate Images"))
        {
            myScript.Generate();
        }

        if (GUILayout.Button("Remove textures"))
        {
            myScript.Clear();
        }
    }
}
