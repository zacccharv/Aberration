using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[Serializable]
public struct Arrows
{
    public GameObject DownArrow;
    public GameObject LeftArrow;
    public GameObject UpArrow;
    public GameObject RightArrow;
}

public class LaneManager : MonoBehaviour
{
    public static LaneManager Instance;

    public delegate void ArrowMoveDelegate(float time);
    public static event ArrowMoveDelegate MoveArrows;

    public List<Arrow> interactableArrows = new();
    public GameObject aberration;
    public Arrows arrowTypes;
    public List<Sprite> aberrationSprites;
    public List<Color> arrowColors = new(), arrowHighlightColor = new();
    public Color SuccessColor, FailColor, FailNumberColor;

    public float moveThresholdFast;
    public float moveThresholdMedium;
    public float moveThresholdLong;
    public Vector2 spawnStart;
    private int _moveCount = 0;

    public float _timer;
    [SerializeField] private List<int> _previousLanes = new();
    [SerializeField] private int _randomArrowThreshold;
    private float _speed;

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
            _speed = MoveSpeed();
            _moveCount++;
            _timer = 0;
        }
    }

    private float MoveSpeed()
    {
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
        GameObject obj = default;

        int lane = GetLaneIndex(_previousLanes);
        _previousLanes.Add(lane);

        int arrowIndex = GetArrowIndex(lane, 6, _randomArrowThreshold);

        Vector2 laneDirection = Vector2.zero;
        Vector2 moveDirection = Vector2.zero;

        // Change directions depending on lane index
        switch (lane)
        {
            case 0:
                laneDirection = Vector2.up;
                moveDirection = Vector2.down;
                break;
            case 1:
                laneDirection = Vector2.right;
                moveDirection = Vector2.left;
                break;
            case 2:
                laneDirection = Vector2.down;
                moveDirection = Vector2.up;
                break;
            case 3:
                laneDirection = Vector2.left;
                moveDirection = Vector2.right;
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
                    arrow = arrowTypes.UpArrow;
                    break;
                case 1:
                    arrow = arrowTypes.RightArrow;
                    break;
                case 2:
                    arrow = arrowTypes.DownArrow;
                    break;
                case 3:
                    arrow = arrowTypes.LeftArrow;
                    break;
                default:
                    break;
            }

            obj = Instantiate(arrow, laneDirection * spawnStart, arrow.transform.rotation, transform);

            obj.GetComponent<SpriteRenderer>().color = arrowColors[arrowIndex];
        }
        else if (arrowIndex >= 4)
        {
            obj = Instantiate(aberration, laneDirection * spawnStart, Quaternion.identity, transform);

            obj.GetComponent<SpriteRenderer>().sprite = aberrationSprites[UnityEngine.Random.Range(0, aberrationSprites.Count)];
            obj.GetComponent<SpriteRenderer>().DOFade(.5f, .33f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        obj.GetComponent<ArrowMovement>().vectorDirection = moveDirection;
        Instance.interactableArrows.Add(obj.GetComponent<Arrow>());

        static int GetArrowIndex(int lane, int maxRandRangeExclusive, int randArrowTriggerThreshold)
        {
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
