using System;
using System.Collections.Generic;
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

    public GameObject aberration;
    public Arrows arrows;
    public List<Sprite> aberrationSprites;
    public List<Color> arrowColors = new(), arrowHighlightColor = new();
    public Color SuccessColor, FailColor;

    public float moveThreshold;
    public Vector2 spawnStart;

    private float _timer;
    [SerializeField] private List<int> _previousLanes = new();
    [SerializeField] private int _randomArrowThreshold;
    public float initialMoveThreshold;

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
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > moveThreshold)
        {
            MoveArrows?.Invoke(moveThreshold);
            SpawnArrow();
            _timer = 0;
        }
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
                    arrow = arrows.UpArrow;
                    break;
                case 1:
                    arrow = arrows.RightArrow;
                    break;
                case 2:
                    arrow = arrows.DownArrow;
                    break;
                case 3:
                    arrow = arrows.LeftArrow;
                    break;
                default:
                    break;
            }

            obj = Instantiate(arrow, laneDirection * spawnStart, arrow.transform.rotation, transform);

            obj.GetComponent<SpriteRenderer>().color = arrowColors[arrowIndex];
        }
        else if (arrowIndex >= 4)
        {
            int aberrationIndex = UnityEngine.Random.Range(0, 4);
            GameObject arrow = default;

            // Pick aberration for lane picked
            switch (aberrationIndex)
            {
                case 0:
                    arrow = arrows.UpArrow;
                    break;
                case 1:
                    arrow = arrows.RightArrow;
                    break;
                case 2:
                    arrow = arrows.DownArrow;
                    break;
                case 3:
                    arrow = arrows.LeftArrow;
                    break;
                default:
                    break;
            }

            obj = Instantiate(aberration, laneDirection * spawnStart, arrow.transform.rotation, transform);

            obj.GetComponent<SpriteRenderer>().sprite = aberrationSprites[UnityEngine.Random.Range(0, aberrationSprites.Count - 1)];
        }

        obj.GetComponent<ArrowMovement>().vectorDirection = moveDirection;

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
                        Debug.Log($"next lane {nextLane}");
                    }
                    else if (has_0 && !has_1 && has_2 && has_3)
                    {
                        nextLane = 1;
                        Debug.Log($"next lane {nextLane}");
                    }
                    else if (has_0 && has_1 && !has_2 && has_3)
                    {
                        nextLane = 2;
                        Debug.Log($"next lane {nextLane}");
                    }
                    else if (has_0 && has_1 && has_2 && !has_3)
                    {
                        nextLane = 3;
                        Debug.Log($"next lane {nextLane}");
                    }

                    previousLanes.Clear();
                }
            }

            return nextLane;
        }
    }
}
