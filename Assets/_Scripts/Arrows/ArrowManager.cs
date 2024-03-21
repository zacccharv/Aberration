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
    public ArrowPrefabs Hold;
}

public class ArrowManager : MonoBehaviour
{
    public static ArrowManager Instance;

    public delegate void ArrowMoveDelegate(float time);
    public static event ArrowMoveDelegate MoveArrows;

    private int _moveCount = 0;
    private float _speed, _timer;

    public Arrows arrows;
    public List<Arrow> interactableArrows = new();
    public List<Sprite> aberrationSprites;
    public List<Color> arrowColors = new(), arrowHighlightColor = new();
    public Color SuccessColor, FailColor, FailNumberColor;

    [SerializeField] private List<int> _previousLanes = new();
    [SerializeField] private int _randomArrowThreshold;

    [HideInInspector] public float moveThresholdFast, moveThresholdLong;


    public float moveThresholdMedium;
    public Vector2 spawnStart;


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
    }

    void Start()
    {
        SpawnArrow();
        Tower.Instance._arrow_0 = interactableArrows[0];
        _speed = MoveSpeed();
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Ended)
        {
            return;
        }

        Instance._timer += Time.deltaTime;

        if (Instance.interactableArrows.Count > 0 && Instance.interactableArrows[0] == null)
        {
            Instance.interactableArrows.RemoveAt(0);
            Tower.Instance._arrow_0 = interactableArrows[0];
        }

        if (_timer > _speed)
        {
            MoveArrows?.Invoke(_speed);
            SpawnArrow();
            _speed = moveThresholdMedium;
            _moveCount++;
            _timer = 0;
        }
    }

    private float MoveSpeed()
    {
        // NOTE don't use different speeds yet
        float value = moveThresholdFast;

        if (_moveCount % 16 == 0)
        {
            value = moveThresholdFast;
        }
        else if (_moveCount % 16 == 11)
        {
            value = moveThresholdMedium;
        }
        else if (_moveCount % 16 == 7 || _moveCount % 16 == 3)
        {
            switch (UnityEngine.Random.Range(0, 3))
            {
                case 0:
                    value = moveThresholdLong;
                    break;
                case 1:
                    value = moveThresholdFast;
                    break;
                case 2:
                    value = moveThresholdMedium;
                    break;
                default:
                    break;
            }
        }

        return value;
    }

    public void SpawnArrow()
    {
        int lane = GetLaneIndex(_previousLanes);
        _previousLanes.Add(lane);

        int arrowIndex = GetArrowIndex(lane, 6, _randomArrowThreshold);

        Vector2 laneDirection = Vector2.zero;

        // Change directions depending on lane index
        switch (lane)
        {
            case 0:
                laneDirection = Vector2.up;
                break;
            case 1:
                laneDirection = Vector2.right;
                break;
            case 2:
                laneDirection = Vector2.down;
                break;
            case 3:
                laneDirection = Vector2.left;
                break;
            default:
                break;
        }

        // Spawn arrow or spawn aberration
        if (arrowIndex < 4)
        {
            GameObject arrow = default;

            // Pick arrow for lane picked
            switch (arrowIndex)
            {
                case 0:
                    arrow = arrows.Single.UpArrow;
                    break;
                case 1:
                    arrow = arrows.Single.RightArrow;
                    break;
                case 2:
                    arrow = arrows.Single.DownArrow;
                    break;
                case 3:
                    arrow = arrows.Single.LeftArrow;
                    break;
                default:
                    break;
            }

            Instantiate(arrow, laneDirection * spawnStart, arrow.transform.rotation, transform);
        }
        else if (arrowIndex >= 4)
        {
            Instantiate(arrows.Empty, laneDirection * spawnStart, Quaternion.identity, transform);
        }

        static int GetArrowIndex(int lane, int maxRandRangeExclusive, int randArrowTriggerThreshold)
        {
            // TODO 2 dimensional index
            int randomizeTrigger = UnityEngine.Random.Range(1, 100);

            if (randomizeTrigger > randArrowTriggerThreshold)
            {
                return UnityEngine.Random.Range(0, maxRandRangeExclusive);
            }
            else
            {
                return lane;
            }
        }

        static int GetLaneIndex(List<int> previousLanes)
        {
            int nextLane = UnityEngine.Random.Range(0, 4);

            if (previousLanes.Count > 0)
            {
                if (previousLanes.Count >= 5)
                {
                    bool has_0 = previousLanes.Contains(0);
                    bool has_1 = previousLanes.Contains(1);
                    bool has_2 = previousLanes.Contains(2);
                    bool has_3 = previousLanes.Contains(3);

                    if (!has_0 && has_1 && has_2 && has_3)
                    {
                        nextLane = 0;
                    }
                    else if (has_0 && !has_1 && has_2 && has_3)
                    {
                        nextLane = 1;
                    }
                    else if (has_0 && has_1 && !has_2 && has_3)
                    {
                        nextLane = 2;
                    }
                    else if (has_0 && has_1 && has_2 && !has_3)
                    {
                        nextLane = 3;
                    }

                    previousLanes.Clear();
                }
            }

            return nextLane;
        }
    }
}
