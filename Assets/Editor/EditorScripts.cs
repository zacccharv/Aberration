using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

class EditorScripts : EditorWindow
{
    [MenuItem("Play/PlayMe _%h")]

    public static void RunMainScene()
    {
        if (!EditorApplication.isPlaying)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            File.WriteAllText(".lastScene", currentSceneName);

            EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
            EditorApplication.isPlaying = true;
        }
        if (EditorApplication.isPlaying)
        {
            string lastScene = File.ReadAllText(".lastScene");
            EditorApplication.isPlaying = false;
            SceneManager.LoadScene(lastScene);
        }
    }

    // void OnGUI()
    // {
    //     // Or set the start Scene from code
    //     var scenePath = "Assets/Scenes/MainMenu.unity";
    //     if (GUILayout.Button("Set start Scene: " + scenePath))
    //         SetPlayModeStartScene(scenePath);
    // }

    // void SetPlayModeStartScene(string scenePath)
    // {
    //     SceneAsset myWantedStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
    //     if (myWantedStartScene != null)
    //         EditorSceneManager.playModeStartScene = myWantedStartScene;
    //     else
    //         Debug.Log("Could not find Scene " + scenePath);
    // }

    // [MenuItem("Test/Open")]
    // static void Open()
    // {
    //     GetWindow<EditorScripts>();
    // }
}