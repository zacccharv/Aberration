using System;
using System.Collections.Generic;
using System.IO;
using Unity.Services.Leaderboards;
using UnityEngine;

[Serializable]
public class Scores
{
    public List<int> highScores = new();
    public string username;
}

public class HighScores : MonoBehaviour
{
    public static event Action ScoresLoaded;
    public static HighScores Instance;
    private string _path;
    public Scores scores;
    const string LeaderboardId = "Up_Down_Left_Right";

    void OnEnable()
    {
        GameManager.GameStateChange += OnGameStateChange;
    }
    void OnDisable()
    {
        GameManager.GameStateChange -= OnGameStateChange;
    }

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(Instance);
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        LoadScoreFile();

        ScoresLoaded?.Invoke();
    }

    public void LoadScoreFile()
    {
        _path = Application.persistentDataPath + "/highScores-score.json";

        if (File.Exists(_path))
        {
            string json = File.ReadAllText(_path);
            Instance.scores = JsonUtility.FromJson<Scores>(json);

            if (Instance.scores.highScores.Count <= 0) Instance.scores.highScores.Add(0);
        }
        else
        {
            Instance.scores.highScores.Add(0);

            File.WriteAllText(_path, JsonUtility.ToJson(Instance.scores, true));
        }
    }

    public void WriteScoreFile()
    {
        string result = JsonUtility.ToJson(Instance.scores, true);

        File.WriteAllText(_path, result);
    }

    public async void AddScore(int score)
    {
        if (scores.highScores.Count == 0)
        {
            Instance.scores.highScores.Add(score);
        }
        else if (score > scores.highScores[^1])
        {
            Instance.scores.highScores.Add(score);
        }

        await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, score);
    }

    public void AddName(string name)
    {
        if (scores.username != "") return;

        Instance.scores.username = name;

        WriteScoreFile();
    }

    private void OnGameStateChange(GameState gameState)
    {
        if (gameState == GameState.Ended)
        {
            AddScore(ScoreManager.Instance.maxScore);

            WriteScoreFile();
        }

        LoadScoreFile();

        ScoresLoaded?.Invoke();
    }
}
