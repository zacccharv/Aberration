using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProfanityFilter : MonoBehaviour
{
    public static ProfanityFilter Instance;
    public string originalFilePath;
    private HashSet<string> profanityWords;

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(Instance);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // GenerateListFile(originalFilePath);
        LoadProfanityWords(originalFilePath);
    }

    // private void GenerateList(string filePath)
    // {
    //     HashSet<string> words = new HashSet<string>();

    //     foreach (var item in File.ReadAllLines(filePath))
    //     {
    //         string[] subStrings;
    //         subStrings = item.Split(",");

    //         foreach (var subItem in subStrings)
    //         {
    //             words.Add(subItem);
    //         }
    //     }

    //     string newPath = Application.dataPath + "/" + Path.GetFileNameWithoutExtension(filePath) + "_copy.txt";
    //     Debug.Log(newPath);

    //     File.WriteAllLines(newPath, words);
    // }

    private void LoadProfanityWords(string filePath)
    {
        Debug.Log(string.Join("\n", File.ReadAllText(filePath).Split(",")));
        profanityWords = new HashSet<string>(File.ReadAllText(filePath).Split(","));
    }

    public bool ContainsProfanity(string text)
    {
        foreach (string word in text.Split(' '))
        {
            if (profanityWords.Contains(word.ToLower()))
            {
                return true;
            }
        }
        return false;
    }

    public string FilterProfanity(string text)
    {
        foreach (string word in text.Split(' '))
        {
            if (profanityWords.Contains(word.ToLower()))
            {
                text = text.Replace(word, new string('*', word.Length));
            }
        }
        return text;
    }
}
