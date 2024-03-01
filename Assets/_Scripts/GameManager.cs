using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action MoveArrows;
    public float moveThreshold;
    [SerializeField] float _timer;
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > moveThreshold)
        {
            MoveArrows?.Invoke();
            _timer = 0;
        }
    }
}
