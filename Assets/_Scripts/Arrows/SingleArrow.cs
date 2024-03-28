using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SingleArrow : MonoBehaviour, IArrowStates
{
    public Arrow Arrow { get; set; }
    public List<Tween> Tweens { get; set; }

    public event KillTweens KillAllTweens;

    public void Success(ScoreType scoreType)
    {
        throw new NotImplementedException();
    }
    public void Fail()
    {
        throw new NotImplementedException();
    }
}
