using System;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject Empty;
    public ArrowPrefabs Single;
    public ArrowPrefabs Double;
    public ArrowPrefabs HoldRight;
    public ArrowPrefabs HoldUp;
    public ArrowPrefabs HoldLeft;
    public ArrowPrefabs HoldDown;
}

public class ArrowManager : MonoBehaviour
{
    public static ArrowManager Instance;

    public delegate void ArrowMoveDelegate(float time);
    public static event ArrowMoveDelegate MoveArrows;
    public static event Action InstanceAwake;

    // private int _moveCount = 0;
    public float _moveSpeed, _moveTimer;

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
        }
        else
        {
            Instance = this;
        }

        InstanceAwake?.Invoke();
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Ended || Instance.interactableArrows.Count < 1)
        {
            return;
        }

        Instance._moveTimer += Time.deltaTime;


        if (Instance.interactableArrows.Count > 0 && Instance.interactableArrows[0] == null)
        {
            Instance.interactableArrows.RemoveAt(0);
            Tower.Instance._arrow_0 = interactableArrows[0];
        }

        if (Instance._moveTimer > _moveSpeed)
        {
            MoveArrows?.Invoke(_moveSpeed);
            Instance._moveTimer = 0;

            _moveSpeed = MoveSpeed(interactableArrows[0]);
        }
    }

    private float MoveSpeed(Arrow arrow)
    {
        float value = arrow.spawnTime;

        return value;
    }

}