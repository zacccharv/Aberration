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
    public int stage;
    public int score = 6, maxScore = 0, comboCount = 0, comboMultiplier = 1;
    [HideInInspector] public int comboType = -1;
    public int _secondsPerStage;
    public GameObject scoreNumberPopup;
    public GameObject stagePopup;
    public int subtraction;
    private int _previousComboMultiplier = 1;
    private int previousStage = 0;
    public List<GameObject> succesfulNumberPopup = new();
    public bool _test;

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
        // Blinks onto screen centered in top half of screen before fade out

        if (!_test) stage = (int)Mathf.Floor(GameManager.Instance.gameTime / _secondsPerStage);

        // LaneManager.Instance.moveThresholdFast = 1 - (.1f * stage);
        // LaneManager.Instance.moveThresholdFast = Mathf.Max(.2f, LaneManager.Instance.moveThresholdFast);

        if (previousStage != stage)
        {
            StagePopUp(stage);
            SpawnSequencing._stage = stage;
        }

        previousStage = stage;
    }

    void AddScore(InteractionType interactionType)
    {
        if (ArrowManager.Instance.interactableArrows[0].inputTriggered
            || ArrowManager.Instance.interactableArrows[0].pressCount == 1
            || GameManager.Instance.gameState == GameState.Ended)
        {
            return;
        }

        if (interactionType == InteractionType.Single && ArrowManager.Instance.interactableArrows[0].TryGetComponent(out SingleArrow _))
        {
            SingleArrow singleArrow = ArrowManager.Instance.interactableArrows[0].GetComponent<SingleArrow>();

            if (singleArrow.perfectInputTimer < singleArrow.perfectInputTime)
            {
                comboCount = 0;
            }
        }

        comboCount++;
        // Animate scaling from 0 to a bit more than full size in center before fade

        if (comboCount >= 60)
        {
            comboMultiplier = 8;

            if (comboCount == 60)
            {
                //SpawnMultiplierIndicator(3);
            }
        }
        else if (comboCount >= 30)
        {
            comboMultiplier = 6;

            if (comboCount == 6)
            {
                //SpawnMultiplierIndicator(2);
            }
        }
        else if (comboCount >= 15)
        {
            comboMultiplier = 4;

            if (comboCount == 15)
            {
                //SpawnMultiplierIndicator(1);
            }
        }
        else if (comboCount >= 5)
        {
            comboMultiplier = 2;

            if (comboCount == 5)
            {
                //SpawnMultiplierIndicator(0);
            }
        }
        else if (comboCount >= 0)
        {
            comboMultiplier = 1;
        }

        if (comboMultiplier != _previousComboMultiplier && _previousComboMultiplier == 1)
        {
            comboType = 1;
            SFXCollection.Instance.PlaySound(SFXType.ComboUp);
        }
        else if (comboCount == 1 && comboType != -1) // if combo started and then reset
        {
            comboType = 2;
            SFXCollection.Instance.PlaySound(SFXType.ComboReset);
        }
        else
        {
            SFXCollection.Instance.PlaySound(SFXType.Success);
        }

        _previousComboMultiplier = comboMultiplier;
        score += 5 * comboMultiplier;

        if (score > maxScore)
        {
            maxScore = score;
        }

        _scoreText.text = score.ToString();
    }

    void SubtractScore()
    {
        subtraction = 3;
        comboType = -1;

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
            GameManager.Instance.ChangeGameStateChange(GameState.Ended);
        }

        _scoreText.text = score.ToString();
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