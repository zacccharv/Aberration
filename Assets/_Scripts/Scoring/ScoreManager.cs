using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ScoreType
{
    Success,
    Empty,
    Fail
}

public class ScoreManager : MonoBehaviour
{
    public delegate void StageDelegate(string videoName);
    public static event StageDelegate StageEvent;

    [SerializeField] TextMeshProUGUI _scoreText;
    public static ScoreManager Instance;
    public int score = 6, maxScore = 0, comboCount = 0, comboMultiplier = 1, secondsPerStage;
    public GameObject scoreNumberPopup, stagePopup;
    public List<GameObject> succesfulNumberPopup = new();
    public bool _test;

    private int _previousComboMultiplier = 1, _previousStage = 1;

    public int stage = 1, subtraction;

    /// <summary>
    /// -1 combo not started, 0 started, 1 comboup, 2 reset
    /// </summary>
    [HideInInspector] public int comboType = -1;
    public bool startStages;

    void OnEnable()
    {
        Tower.InputEvent += SetScoreType;
    }
    void OnDisable()
    {
        Tower.InputEvent -= SetScoreType;
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

        SpawnSequencing._stage = stage;
    }

    private void SetScoreType(ScoreType scoreType, InteractionType interactionType)
    {
        if (scoreType == ScoreType.Success) AddScore();
        else if (scoreType == ScoreType.Fail) SubtractScore();
    }

    void Update()
    {
        // Stage change popup
        stage = _test ? stage + (int)Mathf.Floor(GameManager.Instance.gameTime / secondsPerStage) : (int)Mathf.Floor(GameManager.Instance.gameTime / secondsPerStage);

        if (_previousStage != stage && !_test && startStages)
        {
            StagePopUp(stage);
            SpawnSequencing._stage = stage;
        }
        else if (!startStages)
        {
            SpawnSequencing._stage = stage;
            return;
        }

        _previousStage = stage;
    }

    void AddScore()
    {
        if (ArrowManager.Instance.interactableArrows[0].inputTriggered
            || GameManager.Instance.gameState == GameState.Ended)
        {
            return;
        }

        if (ArrowManager.Instance.interactableArrows[0].interactionType == InteractionType.Double && !ArrowManager.Instance.interactableArrows[0].inputTriggered)
        {
            return;
        }

        comboCount++;
        // TODO Animate scaling from 0 to a bit more than full size in center before fade

        // multiplier setter
        switch (comboCount)
        {
            case >= 60:
                comboMultiplier = 8;
                break;
            case >= 30:
                comboMultiplier = 6;
                break;
            case >= 15:
                comboMultiplier = 4;
                break;
            case >= 5:
                comboMultiplier = 2;
                break;
            case >= 0:
                comboMultiplier = 1;
                break;
        }

        // if combo started combo multiplier went up and perfect input hit then comboup
        if (comboMultiplier != _previousComboMultiplier && comboMultiplier > 1)
        {
            comboType = 1;
            SFXCollection.Instance.PlaySound(SFXType.ComboUp);
        }
        // TODO: if you break combo after speed shrink at max -> subtract score
        // if combo started and perfect input missed then reset
        else if (comboCount == 1 && comboType != -1)
        {
            if (GameManager.Instance.speedExpand >= .4)
            {
                SetScoreType(ScoreType.Fail, InteractionType.NoPress);
                return;
            }
            else
            {
                comboType = 2;
                SFXCollection.Instance.PlaySound(SFXType.ComboReset);
            }
        }
        // if combo not reset or combo up
        else
        {
            SFXCollection.Instance.PlaySound(SFXType.Success);
            // if combo started combotype set to 0 right away
            if (comboCount == 1) comboType = 0;
        }

        // set new _previousComboMultiplier, initializes to combomult
        _previousComboMultiplier = comboMultiplier;

        score += 5 * comboMultiplier;

        // check if highest score this playthrough
        if (score > maxScore)
        {
            maxScore = score;
        }

        GameManager.Instance.SetTimeScale(maxScore);
        _scoreText.text = score.ToString();
    }

    void SubtractScore()
    {
        // TODO subtraction amount indicator
        // NOTE subtraction algorithm
        subtraction = Mathf.FloorToInt(maxScore / 4);

        score -= subtraction;
        score = Mathf.Max(0, score);

        // Reset all necessary vars
        comboType = -1;
        comboCount = 0;
        comboMultiplier = 1;
        _previousComboMultiplier = 1;

        _scoreText.text = score.ToString();

        if (score == 0)
        {
            GameManager.Instance.ChangeGameState(GameState.Ended);
        }
    }

    private void StagePopUp(int stage)
    {
        if (GameManager.Instance.gameState == GameState.Ended)
        {
            return;
        }

        GameObject obj = Instantiate(stagePopup, new(0, 3.5f, 0), Quaternion.identity);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = $"Stage {stage + 1}";

        if (stage + 1 == 2)
        {
            Invoke(nameof(InvokeStage_2), GameManager.Instance.GetTimeScale() * 5);
        }
        else if (stage + 1 == 3)
        {
            Invoke(nameof(InvokeStage_3), GameManager.Instance.GetTimeScale() * 5);
        }

    }

    public void InvokeStage_2()
    {
        StageEvent?.Invoke("Double Tutorial");
    }
    public void InvokeStage_3()
    {
        StageEvent?.Invoke("Long Tutorial");
    }

}