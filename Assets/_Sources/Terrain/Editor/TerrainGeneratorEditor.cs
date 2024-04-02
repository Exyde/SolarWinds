using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainGenerator _terrainGenerator = (TerrainGenerator)target;
        if (DrawDefaultInspector()){
            if (_terrainGenerator._autoUpdate){
                //_terrainGenerator.GenerateTerrainData();
            }
        }

        if (GUILayout.Button("Generate Terrain Data")){
            _terrainGenerator.GenerateTerrainData();
        }

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate Forest")){
            _terrainGenerator.GenerateForest();
        }

        if (GUILayout.Button("Clear Forest")){
            _terrainGenerator.ClearForest();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate Fireflies")){
            _terrainGenerator.GenerateFireflies();
        }

        GUILayout.EndHorizontal();
    }
}
