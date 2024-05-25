using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[Serializable]
public struct LogEntry
{
    // Unity specific
    public string InputPhase;
    public float ExpectedHoldTime;
    public float ActualHoldTime;
    public float DoublePressWindow;
    public float ActualPressWindow;

    // Game specific
    public string InteractionType;
    public string Direction;
    public string ScoreType;
    public bool PerfectInput;
    public LogEntry(InputActionPhase inputActionPhase,
                    InteractionType interactionType,
                    Direction direction,
                    ScoreType scoreType,
                    float expectedHoldTime,
                    float actualHoldTime,
                    float doublePressWindow,
                    float actualPressWindow,
                    bool perfectInput)
    {
        InputPhase = inputActionPhase.ToString();
        InteractionType = interactionType.ToString();
        Direction = direction.ToString();
        ScoreType = scoreType.ToString();
        ExpectedHoldTime = expectedHoldTime;
        ActualHoldTime = actualHoldTime;
        DoublePressWindow = doublePressWindow;
        ActualPressWindow = actualPressWindow;
        PerfectInput = perfectInput;
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

    public void AddToLog(InputAction.CallbackContext callbackContext, InteractionType interactionType, Direction direction, ScoreType scoreType, bool perfectInput)
    {
        float expectedHold = -1, actualHold = -1, doublePressWindow = -1, actualPressWindow = -1;

        if (callbackContext.interaction is HoldInteraction)
        {
            expectedHold = (callbackContext.interaction as HoldInteraction).duration;
            actualHold = (float)callbackContext.duration;
        }
        else if (callbackContext.interaction is MultiTapInteraction)
        {
            doublePressWindow = (callbackContext.interaction as MultiTapInteraction).tapDelay;
            actualPressWindow = Time.realtimeSinceStartup - (float)callbackContext.time;
        }

        LogEntry logEntry = new(callbackContext.phase, interactionType, direction, scoreType, expectedHold, actualHold, doublePressWindow, actualPressWindow, perfectInput);

        _logEntries.LogEntries.Add(logEntry);
    }
}