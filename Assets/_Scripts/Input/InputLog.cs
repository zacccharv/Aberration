using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[Serializable]
public struct InputData
{
    public string InputPhase;
    public string InteractionType;
    public string ButtonDirection;
    public float ExpectedHoldTime;
    public float ActualHoldTime;
    public float DoublePressWindow;
    public float ActualPressWindow;
}

[Serializable]
public struct ArrowData
{
    public string ArrowInteraction;
    public string ArrowDirection;
    public string ScoreType;
    public bool PerfectInput;
}

[Serializable]
public struct LogEntry
{
    public InputData InputData;
    public ArrowData ArrowData;
    public LogEntry(InputActionPhase inputActionPhase,
                    InteractionType interactionType,
                    Direction buttonDirection,
                    Direction arrowDirection,
                    InteractionType arrowInteraction,
                    ScoreType scoreType,
                    float expectedHoldTime,
                    float actualHoldTime,
                    float doublePressWindow,
                    float actualPressWindow,
                    bool perfectInput)
    {
        InputData.InputPhase = inputActionPhase.ToString();
        InputData.InteractionType = interactionType.ToString();
        InputData.ButtonDirection = buttonDirection.ToString();
        InputData.ExpectedHoldTime = expectedHoldTime;
        InputData.ActualHoldTime = actualHoldTime;
        InputData.DoublePressWindow = doublePressWindow;
        InputData.ActualPressWindow = actualPressWindow;

        ArrowData.ScoreType = scoreType.ToString();
        ArrowData.ArrowDirection = arrowDirection.ToString();
        ArrowData.ArrowInteraction = arrowInteraction.ToString();
        ArrowData.PerfectInput = perfectInput;
    }

}

[Serializable]
public class LogList
{
    public List<LogEntry> LogEntries = new();
}

public class InputLog : MonoBehaviour
{
    string _path;
    [SerializeField] private LogList _logEntries;

    void OnEnable()
    {
        GameManager.GameStateChange += WriteScoreFile;
    }
    void OnDisable()
    {
        GameManager.GameStateChange -= WriteScoreFile;
    }

    void Awake()
    {
        LoadLog();
    }

    public void LoadLog()
    {
        File.Delete(Application.persistentDataPath + "/input-log.json");
        _path = Application.persistentDataPath + "/input-log.json";

        if (File.Exists(_path) && File.ReadAllText(_path) != "")
        {
            string json = File.ReadAllText(_path);

            _logEntries = JsonUtility.FromJson<LogList>(json);

            if (_logEntries.LogEntries.Count <= 0) _logEntries.LogEntries.Add(default);
        }
        else
        {
            _logEntries.LogEntries = new List<LogEntry>();

            File.WriteAllText(_path, JsonUtility.ToJson(_logEntries, true));
        }
    }

    public void WriteScoreFile(GameState _)
    {
        if (_ == GameState.Ended || _ == GameState.Paused)
        {
            _path = Application.persistentDataPath + "/input-log.json";

            string result = JsonUtility.ToJson(_logEntries, true);

            File.WriteAllText(_path, result);
        }
    }

    public void AddToLog(InputAction.CallbackContext callbackContext,
                         InteractionType btnInteractionType,
                         Direction btnDirection,
                         Direction arrowDirection,
                         InteractionType arrowInteractionType,
                         ScoreType scoreType,
                         bool perfectInput)
    {
        float expectedHold = -1, actualHold = -1, doublePressWindow = -1, actualPressWindow = -1;

        if (callbackContext.interaction is HoldInteraction)
        {
            expectedHold = (callbackContext.interaction as HoldInteraction).duration;
            actualHold = (float)callbackContext.duration;
        }
        else if (callbackContext.interaction is MultiTapInteraction)
        {
            doublePressWindow = .666f;
            actualPressWindow = Time.realtimeSinceStartup - (float)callbackContext.time;
        }

        LogEntry logEntry = new(callbackContext.phase,
                                btnInteractionType,
                                btnDirection,
                                arrowDirection,
                                arrowInteractionType,
                                scoreType,
                                expectedHold,
                                actualHold,
                                doublePressWindow,
                                actualPressWindow,
                                perfectInput);

        _logEntries.LogEntries.Add(logEntry);
    }
}
