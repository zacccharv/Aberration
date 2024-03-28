using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoubleArrow : MonoBehaviour, IArrowStates
{
    public event KillTweens KillAllTweens;
    public Arrow Arrow { get; set; }
    public List<Tween> Tweens { get; set; }


    void OnEnable()
    {
        Tower.SuccessfulInput += Success;
        Tower.FailedInput += Fail;
        KillAllTweens += ArrowManager.KillAllTweens;
    }
    void OnDisable()
    {
        Tower.SuccessfulInput -= Success;
        Tower.FailedInput -= Fail;
        KillAllTweens -= ArrowManager.KillAllTweens;
    }
    public void Success(ScoreType scoreType)
    {
        throw new System.NotImplementedException();
    }
    public void Fail()
    {
        throw new System.NotImplementedException();
    }
}
