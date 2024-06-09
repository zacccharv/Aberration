using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class MenuItemExtras : MonoBehaviour
{

    /// <summary>
    /// <term>Method</term>
    /// Create a new C# script in specific folder
    /// </summary>
    [MenuItem("Create/New Script _&n")]
    static void NewScript()
    {
        string path = Application.dataPath;
        string result = EditorUtility.SaveFilePanel("Choose Folder", path, "", "cs");

        if (result == "")
            return;

        result = string.Join("", result);

        // Write preformatted template to new file
        File.WriteAllText(result,
        "using UnityEngine;\n"
        + "\n"
        + $"public class {Path.GetFileNameWithoutExtension(result)}: MonoBehaviour\n"
        + "{\n"
        + "\n"
        + "    public void OnEnable()\n"
        + "    {\n"
        + "        // subscribe here\n"
        + "    }\n"
        + "    public void OnDisable()\n"
        + "    {\n"
        + "        // unsubscribe here\n"
        + "    }\n"
        + "\n"
        + "    public void Awake()\n"
        + "    {\n"
        + "        // init here\n"
        + "    }\n"
        + "\n"
        + "}"
        );

        AssetDatabase.Refresh();
    }
}
