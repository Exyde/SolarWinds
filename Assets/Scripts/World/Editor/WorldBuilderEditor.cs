using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldBuilder))]
public class WorldBuilderEditor : Editor
{
    WorldBuilder _worldBuilder;

    public override void OnInspectorGUI() {
        _worldBuilder = (WorldBuilder)target;

        base.OnInspectorGUI();

        EditorGUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate World")){
            _worldBuilder.GenerateWorld();
        }

        if (GUILayout.Button("Clear World")){
            _worldBuilder.ClearWorld();
        }
        EditorGUILayout.EndHorizontal();

    }
}
