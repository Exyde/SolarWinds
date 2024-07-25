using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainSurface))]
public class TerrainSurfaceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainSurface terrainSurface = (TerrainSurface)target;
        
        if (GUILayout.Button("Generate Terrain Data")){
            terrainSurface.GenerateTerrainData();
        }
    }
}
