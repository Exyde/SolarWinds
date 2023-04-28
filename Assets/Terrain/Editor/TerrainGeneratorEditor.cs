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
                _terrainGenerator.GenerateTerrainData();
            }
        }

        if (GUILayout.Button("Generate Terrain Data")){
            _terrainGenerator.GenerateTerrainData();
        }
    }
}
