using System;
using TMPro;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public static Scoring Instance;
    [SerializeField] TextMeshProUGUI _scoreText;
    public int score = 0, comboCount = 0, stage = 0;
    [SerializeField] int _secondsPerStage;

    void OnEnable()
    {
        Tower.SuccesfulInput += AddScore;
        Tower.FailedInput += SubtractScore;
    }
    void OnDisable()
    {
        Tower.SuccesfulInput -= AddScore;
        Tower.FailedInput -= SubtractScore;
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

    void Update()
    {
        stage = (int)Mathf.Floor(GameManager.Instance.gameTime / _secondsPerStage);

        if (stage == 2 && LaneManager.Instance.moveThreshold != LaneManager.Instance.initialMoveThreshold)
        {
            LaneManager.Instance.moveThreshold = .85f;
        }
    }

    void AddScore()
    {
        comboCount++;

        if (comboCount >= 10)
        {
            score += 4;
        }
        else if (comboCount >= 5)
        {
            score += 2;
        }
        else if (comboCount >= 0)
        {
            score++;
        }

        _scoreText.text = score.ToString();
    }

    void SubtractScore()
    {
        // NOTE exponential score loss

        if (GameManager.Instance.gameTime >= 10)
        {
            score -= 6;
        }
        else if (GameManager.Instance.gameTime >= 5)
        {
            score -= 4;
        }
        else if (GameManager.Instance.gameTime >= 0)
        {
            score--;
        }

        score = Mathf.Max(0, score);
        comboCount = 0;

        _scoreText.text = score.ToString();
    }
}
