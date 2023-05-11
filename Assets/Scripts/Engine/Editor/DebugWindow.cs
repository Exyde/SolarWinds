using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugWindow : EditorWindow {

    [MenuItem("Tools/Debug Window")]
    public static void Init(){
        DebugWindow window = GetWindow<DebugWindow>("Debug Editor Menu");
    }
 
    private void OnGUI() {
        if (GUILayout.Button("Select Player")){
            Selection.activeGameObject = GameObject.FindGameObjectWithTag("Player");
        }

        if (GUILayout.Button("Teleport Player to World Center")){
            Selection.activeGameObject = GameObject.FindGameObjectWithTag("Player");
            Selection.activeGameObject.transform.position = new Vector3(0, 50f, 0);
        }

        //Thats meh
        /*
        if (GUILayout.Button("Multi-Selection Rename")){
            string baseName = Selection.gameObjects[0].name;
            for (int i =0; i < Selection.gameObjects.Length; i++){
                Selection.gameObjects[i].name = $"{baseName} - {i}";
            }
        }
        */

        GUI.enabled = Application.isPlaying; //Only play mode option
        if (GUILayout.Button("Reload Scene")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        GUILayout.Space(20);

        if (GUILayout.Button($"Game speed to normal [TimeScale : {Time.timeScale}]")) Time.timeScale = 1;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Game speed --")) Time.timeScale -= 0.25f;
        if (GUILayout.Button("Game speed ++")) Time.timeScale += 0.25f;
        GUILayout.EndHorizontal();
    }
}
