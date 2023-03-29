using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateLevelGeometry))]
public class CreateLevelGeometryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CreateLevelGeometry myScript = (CreateLevelGeometry)target;
        if (GUILayout.Button("Build Level"))
        {
            myScript.ShowUI();
        }
    }
}
