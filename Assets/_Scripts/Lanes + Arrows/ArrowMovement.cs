using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    private Tween tween;
    [SerializeField] float _physicalDistance = 1;
    public Vector2 vectorDirection;

    void OnEnable()
    {
        LaneManager.MoveArrows += Move;
        ArrowStateMachines.KillTweens += KillAllTweens;
    }
    void OnDisable()
    {
        LaneManager.MoveArrows -= Move;
        ArrowStateMachines.KillTweens -= KillAllTweens;
    }

    private void Move(float time)
    {
        tween = transform.DOMove(transform.position + ((Vector3)vectorDirection * _physicalDistance), time / 2).SetEase(Ease.InOutSine);
    }

    private void KillAllTweens()
    {
        tween.Kill();
    }

}
