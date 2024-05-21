using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;

class EditorScrips : EditorWindow
{
    [MenuItem("Play/PlayMe _%h")]

    public static void RunMainScene()
    {
        if (!EditorApplication.isPlaying)
        {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            File.WriteAllText(".lastScene", currentSceneName);

            EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
            EditorApplication.isPlaying = true;
        }
        if (EditorApplication.isPlaying)
        {
            string lastScene = File.ReadAllText(".lastScene");
            EditorApplication.isPlaying = false;
            EditorSceneManager.OpenScene(lastScene);
        }
    }
}