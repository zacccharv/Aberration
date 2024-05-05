using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Scores
{
    public List<int> highScores = new();
    public string username;
}

[RequireComponent(typeof(DontDestroy))]
public class HighScores : MonoBehaviour
{
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
        }
        else
        {
            Instance = this;
        }
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

    public async void AddScore(int score)
    {
        if (score > scores.highScores[^1])
        {
            scores.highScores.Add(score);
        }

        await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, score);
    }

    public void AddName(string name)
    {
        if (scores.username != "") return;

        scores.username = name;

        WriteScoreFile();
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
