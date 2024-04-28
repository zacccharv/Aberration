using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Scores
{
    public List<int> highScores = new();
}

public class HighScores : MonoBehaviour
{
    private string _path;
    [SerializeField] private Scores scores;

    void OnEnable()
    {
        GameManager.GameStateChange += OnGameStateChange;
    }
    void OnDisable()
    {
        GameManager.GameStateChange -= OnGameStateChange;
    }

    void Start()
    {
        LoadScoreFile();
    }

    public void LoadScoreFile()
    {
        _path = Application.dataPath + "/highScores-score.json";

        if (File.Exists(_path))
        {
            string json = File.ReadAllText(_path);
            scores = JsonUtility.FromJson<Scores>(json);

            if (scores.highScores.Count <= 0) scores.highScores.Add(0);
        }
        else
        {
            scores.highScores.Add(0);

            File.WriteAllText(_path, JsonUtility.ToJson(scores, true));
        }
    }

    public void WriteScoreFile()
    {
        string result = JsonUtility.ToJson(scores, true);

        File.WriteAllText(_path, result);
    }

    public void AddScore(int score)
    {
        if (score > scores.highScores[^1])
        {
            scores.highScores.Add(score);
        }
    }

    private void OnGameStateChange(GameState gameState)
    {
        if (gameState == GameState.Ended)
        {
            AddScore(ScoreManager.Instance.maxScore);
            WriteScoreFile();
        }
    }
}
