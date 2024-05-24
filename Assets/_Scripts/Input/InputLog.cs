using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct LogEntry
{
    // Unity specific
    public InputActionPhase UnityInputPhase;
    public float TimingExpected;
    public float ActualTime;

    // Game specific
    public InteractionType interactionType;
    public Direction Direction;
    public ScoreType ScoreType;


}

public class InputLog : MonoBehaviour
{
}
