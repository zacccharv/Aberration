using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ScoreType
{
    SinglePress,
    DoublePress,
    LongPress,
    Empty
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] TextMeshProUGUI _scoreText;
    public int score = 6, comboCount = 0, comboMultiplier = 1, stage = 0;
    public int _secondsPerStage;
    public GameObject scoreNumberPopup;
    public List<GameObject> comboMultiplierPopups = new();
    public GameObject stagePopup;
    public int subtraction;
    private int previousStage = 0;

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

        // LaneManager.Instance.moveThresholdFast = 1 - (.1f * stage);
        // LaneManager.Instance.moveThresholdFast = Mathf.Max(.2f, LaneManager.Instance.moveThresholdFast);

        if (previousStage != stage)
        {
            StagePopUp(stage);
        }

        previousStage = stage;
    }


    void AddScore(ScoreType scoreType)
    {
        if (scoreType == ScoreType.Empty || ArrowManager.Instance.interactableArrows[0].isPressed || GameManager.Instance.gameState == GameState.Ended)
        {
            return;
        }

        comboCount++;
        // TODO Combo Multiplier Popup
        // Animate scaling from 0 to a bit more than full size in center before fade

        if (comboCount >= 60)
        {
            comboMultiplier = 8;

            if (comboCount == 60)
            {
                SpawnMultiplierIndicator(3);
            }
        }
        else if (comboCount >= 30)
        {
            comboMultiplier = 6;

            if (comboCount == 6)
            {
                SpawnMultiplierIndicator(2);
            }
        }
        else if (comboCount >= 15)
        {
            comboMultiplier = 4;

            if (comboCount == 15)
            {
                SpawnMultiplierIndicator(1);
            }
        }
        else if (comboCount >= 5)
        {
            comboMultiplier = 2;

            if (comboCount == 5)
            {
                SpawnMultiplierIndicator(0);
            }
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
        subtraction = 2;

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

        if (score == 0)
        {
            GameManager.Instance.OnGameStateChange(GameState.Ended);
        }

        _scoreText.text = score.ToString();
    }

    private void SpawnMultiplierIndicator(int index)
    {
        Instantiate(comboMultiplierPopups[index], new(0, 0, 0), Quaternion.identity);
    }
    private void StagePopUp(int stage)
    {
        if (GameManager.Instance.gameState == GameState.Ended)
        {
            return;
        }

        GameObject obj = Instantiate(stagePopup, new(0, 3.5f, 0), Quaternion.identity);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = $"Stage {stage + 1}";
    }
}