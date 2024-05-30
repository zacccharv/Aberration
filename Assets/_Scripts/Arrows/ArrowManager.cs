using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public delegate void KillTweens(List<Tween> tweens);

[Serializable]
public struct ArrowPrefabs
{
    public GameObject UpArrow;
    public GameObject RightArrow;
    public GameObject DownArrow;
    public GameObject LeftArrow;
}

[Serializable]
public struct Arrows
{
    public ArrowPrefabs Empty;
    public ArrowPrefabs Single;
    public ArrowPrefabs Double;
    public ArrowPrefabs HoldRightLane;
    public ArrowPrefabs HoldUpLane;
    public ArrowPrefabs HoldLeftLane;
    public ArrowPrefabs HoldDownLane;
}

public class ArrowManager : MonoBehaviour
{
    public static ArrowManager Instance;

    public delegate void ArrowMoveDelegate(float time);
    public static event ArrowMoveDelegate MoveArrows;
    public static event Action InstanceAwake;

    // private int _moveCount = 0;
    private float _moveTimer;
    public float moveSpeed;

    public Arrows arrows;
    public List<Arrow> interactableArrows = new();
    public List<Sprite> aberrationSprites;
    public List<Color> arrowColors = new(), arrowHighlightColor = new();
    public Color SuccessColor, FailColor, FailNumberColor;

    [HideInInspector] public float moveThresholdFast, moveThresholdLong;

    public float moveThresholdMedium;

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(Instance);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        InstanceAwake?.Invoke();
    }

    void Update()
    {
        if (Instance.interactableArrows.Count < 1)
        {
            return;
        }

        Instance._moveTimer += GameManager.deltaTime;

        if (Instance.interactableArrows.Count > 0 && Instance.interactableArrows[0] == null)
        {
            Instance.interactableArrows.RemoveAt(0);
        }

        if (Instance._moveTimer > moveSpeed)
        {
            moveSpeed = MoveSpeed(interactableArrows[0]);

            MoveArrows?.Invoke(moveSpeed);
            Instance._moveTimer = 0;
        }
    }

    private float MoveSpeed(Arrow arrow)
    {
        float value = arrow.moveSpeed * GameManager.timeScale;

        return value;
    }

}