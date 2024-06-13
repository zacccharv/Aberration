using System;
using System.Collections.Generic;
using System.IO;
using Unity.Services.Leaderboards;
using UnityEngine;

[Serializable]
public class Scores
{
    public List<int> highScores = new() { };
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
        _path = Application.persistentDataPath + "/highScores.json";

        LoadScoreFile();

        ScoresLoaded?.Invoke();
    }

    public void LoadScoreFile()
    {
        _path = Application.persistentDataPath + "/highScores.json";

        UnityFileManipulation.LoadJsonFile(_path, out Instance.scores);
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
        if (Instance.scores.username != "" && Instance.scores.username != null) return;

        Instance.scores.username = name;

        UnityFileManipulation.WriteJsonFile(_path, Instance.scores);
    }

    private void OnGameStateChange(GameState gameState)
    {
        // NOTE only adds score on end -- changed 6/12/24
        // NOTE loads score on start and end -- changed 6/12/24
        if (gameState == GameState.Ended)
        {
            AddScore(ScoreManager.Instance.maxScore);

            // NOTE waits until highscore is online -- changed 6/12/24
            Invoke(nameof(HurryUpAndWait), .2f);

            return;
        }
        else if (gameState != GameState.Started || gameState != GameState.Paused || gameState != GameState.Tutorial)
        {
            LoadScoreFile();
            ScoresLoaded?.Invoke();
        }
    }

    public void HurryUpAndWait()
    {
        UnityFileManipulation.WriteJsonFile(_path, Instance.scores);
        LoadScoreFile();

        ScoresLoaded?.Invoke();
    }
}
