using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    MapGenerator mapGenerator;
    Editor particleEditor;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Initialize"))
        {
            mapGenerator.InitializeGround();
        }
    }
    private void OnEnable()
    {
        mapGenerator = (MapGenerator)target;
    }
}
