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

    void Start()
    {
        Tower.Instance._arrow_0 = interactableArrows[0];
        _moveSpeed = MoveSpeed(interactableArrows[0]);
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Ended)
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

            // _moveSpeed = moveThresholdMedium;
            // _moveCount++;
        }
    }

    private float MoveSpeed(Arrow arrow)
    {
        float value = arrow.spawnTime;

        // NOTE don't use different speeds yet
        // float value = moveThresholdFast;

        // if (_moveCount % 16 == 0)
        // {
        //     value = moveThresholdFast;
        // }
        // else if (_moveCount % 16 == 11)
        // {
        //     value = moveThresholdMedium;
        // }
        // else if (_moveCount % 16 == 7 || _moveCount % 16 == 3)
        // {
        //     switch (UnityEngine.Random.Range(0, 3))
        //     {
        //         case 0:
        //             value = moveThresholdLong;
        //             break;
        //         case 1:
        //             value = moveThresholdFast;
        //             break;
        //         case 2:
        //             value = moveThresholdMedium;
        //             break;
        //         default:
        //             break;
        //     }
        // }

        return value;
    }

}