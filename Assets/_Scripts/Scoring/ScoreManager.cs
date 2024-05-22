using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ScoreType
{
    Press,
    Empty,
    Fail
}

public class ScoreManager : MonoBehaviour
{
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
        if (_test) score = 100000;
    }

    private void SetScoreType(ScoreType scoreType, InteractionType interactionType)
    {
        if (scoreType == ScoreType.Press) AddScore(interactionType);
        else if (scoreType == ScoreType.Fail) SubtractScore();
    }

    void Update()
    {
        // Stage change popup
        stage = _test ? stage + (int)Mathf.Floor(GameManager.Instance.gameTime / secondsPerStage) : (int)Mathf.Floor(GameManager.Instance.gameTime / secondsPerStage);

        if (_previousStage != stage)
        {
            StagePopUp(stage);
            SpawnSequencing._stage = stage;
        }

        _previousStage = stage;
    }

    void AddScore(InteractionType interactionType)
    {
        if (ArrowManager.Instance.interactableArrows[0].inputTriggered
            || ArrowManager.Instance.interactableArrows[0].pressCount == 1
            || GameManager.Instance.gameState == GameState.Ended)
        {
            return;
        }

        // Combo count reset for single arrow
        if (interactionType == InteractionType.Single && ArrowManager.Instance.interactableArrows[0].TryGetComponent(out SingleArrow _))
        {
            SingleArrow singleArrow = ArrowManager.Instance.interactableArrows[0].GetComponent<SingleArrow>();

            if (singleArrow.perfectInputTimer < singleArrow.perfectInputTime)
            {
                comboCount = 0;
            }
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
        // if combo started and perfect input missed then reset
        else if (comboCount == 1 && comboType != -1)
        {
            comboType = 2;
            SFXCollection.Instance.PlaySound(SFXType.ComboReset);
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
    }
}