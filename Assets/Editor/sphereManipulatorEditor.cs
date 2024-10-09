using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SphereManipulator))]

public class SphereManipulatorEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SphereManipulator sphere = (SphereManipulator)target;
        if(GUILayout.Button("Generate"))
        {
            sphere.SendMessage("Generate");
        }
    }
}
