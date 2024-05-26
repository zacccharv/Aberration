using System;
using System.IO;
using UnityEngine;

public static class UnityFileManipulation
{
    public static void LoadJsonFile<T>(string path, out T type)
    {
        if (File.Exists(path) && File.ReadAllText(path) != "")
        {
            string json = File.ReadAllText(path);

            type = JsonUtility.FromJson<T>(json);
        }
        else
        {
            type = (T)Activator.CreateInstance(typeof(T));

            File.WriteAllText(path, JsonUtility.ToJson(path, true));
        }
    }

    public static void WriteJsonFile<T>(string path, T data)
    {
        string result = JsonUtility.ToJson(data);

        File.WriteAllText(path, result);
    }
}