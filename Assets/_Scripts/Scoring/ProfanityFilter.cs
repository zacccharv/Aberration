using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProfanityFilter : MonoBehaviour
{
    private HashSet<string> profanityWords;

    void Start()
    {
        LoadProfanityWords("profanity.txt");
    }

    private void LoadProfanityWords(string filePath)
    {
        profanityWords = new HashSet<string>(File.ReadAllLines(filePath));
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
