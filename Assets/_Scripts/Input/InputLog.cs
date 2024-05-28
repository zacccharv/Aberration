using System;
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
    public float TimeInSecs;
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
                    float timeInSecs,
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

        TimeInSecs = timeInSecs;
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
        Application.quitting += WriteScoreFile;
    }
    void OnDisable()
    {
        Application.quitting -= WriteScoreFile;
    }

    void Awake()
    {
        _path = Application.persistentDataPath + "/input-log.json";
        File.Delete(Application.persistentDataPath + "/input-log.json");

        UnityFileManipulation.LoadJsonFile(_path, out _logEntries);
    }

    public void WriteScoreFile()
    {
        _path = Application.persistentDataPath + "/input-log.json";

        UnityFileManipulation.WriteJsonFile(_path, _logEntries);
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
                                Time.realtimeSinceStartup,
                                perfectInput);

        _logEntries.LogEntries.Add(logEntry);
    }

}
