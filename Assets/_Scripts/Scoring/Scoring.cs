using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum ScoreType
{
    Direction,
    Empty
}

public class Scoring : MonoBehaviour
{
    public static Scoring Instance;
    [SerializeField] TextMeshProUGUI _scoreText;
    public int score = 0, comboCount = 0, comboMultiplier = 1, stage = 0;
    public int _secondsPerStage;
    public GameObject scoreNumberPopup;
    public int subtraction;

    void OnEnable()
    {
        Tower.SuccessfulInput += AddScore;
        Tower.FailedInput += SubtractScore;
    }
    void OnDisable()
    {
        Tower.SuccessfulInput -= AddScore;
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
        // TODO stage change popup
        // Blinks onto screen centered in top half of screen before fade out

        stage = (int)Mathf.Floor(GameManager.Instance.gameTime / _secondsPerStage);

        LaneManager.Instance.moveThreshold = 1 - (.05f * stage);

    }

    void AddScore(ScoreType scoreType)
    {
        if (scoreType == ScoreType.Empty || Tower.Instance.inputPressed)
        {
            Debug.Log("No Score but success");
            return;
        }

        comboCount++;
        // TODO Combo Multiplier Popup
        // Animate scaling from 0 to a bit more than full size in center before fade

        if (comboCount >= 60)
        {
            comboMultiplier = 8;
        }
        else if (comboCount >= 30)
        {
            comboMultiplier = 6;
        }
        else if (comboCount >= 15)
        {
            comboMultiplier = 4;
        }
        else if (comboCount >= 5)
        {
            comboMultiplier = 2;
        }
        else if (comboCount >= 0)
        {
            comboMultiplier = 1;
        }

        score += 5 * comboMultiplier;

        _scoreText.text = score.ToString();
    }

    void SubtractScore()
    {
        // NOTE exponential score loss
        subtraction = 0;

        subtraction = (int)Mathf.Pow(2, stage + 1);

        if (comboCount >= 60)
        {
            subtraction += 300;
        }
        else if (comboCount >= 30)
        {
            subtraction += 100;
        }

        score -= subtraction;
        score = Mathf.Max(0, score);
        comboCount = 0;
        comboMultiplier = 1;

        _scoreText.text = score.ToString();
    }
}
