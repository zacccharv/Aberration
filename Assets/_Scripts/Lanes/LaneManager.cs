using System;
using System.Collections.Generic;
using UnityEditor;
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

    public static event Action MoveArrows;

    public GameObject aberration;
    public Arrows arrows;
    public List<Sprite> aberrationSprites;
    public List<Color> arrowColors;

    public float moveThreshold;
    public Vector2 spawnStart;

    private float _timer;

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
            MoveArrows?.Invoke();
            SpawnArrow();
            _timer = 0;
        }
    }

    public void SpawnArrow()
    {
        GameObject obj = default;

        int aberrationIndex = -1;
        int lane = UnityEngine.Random.Range(0, 4);

        int arrowIndex = GetArrowIndex(lane);

        Vector2 laneDir = Vector2.zero;
        Vector2 moveDirection = Vector2.zero;

        if (lane == 0)
        {
            laneDir = Vector2.up;
            moveDirection = Vector2.down;
        }
        else if (lane == 1)
        {
            laneDir = Vector2.right;
            moveDirection = Vector2.left;
        }
        else if (lane == 2)
        {
            laneDir = Vector2.down;
            moveDirection = Vector2.up;
        }
        else if (lane == 3)
        {
            laneDir = Vector2.left;
            moveDirection = Vector2.right;
        }

        if (arrowIndex < 4)
        {
            if (arrowIndex == 0)
            {
                obj = Instantiate(arrows.DownArrow, laneDir * spawnStart, arrows.DownArrow.transform.rotation, transform);
            }
            else if (arrowIndex == 1)
            {
                obj = Instantiate(arrows.LeftArrow, laneDir * spawnStart, arrows.LeftArrow.transform.rotation, transform);
            }
            else if (arrowIndex == 2)
            {
                obj = Instantiate(arrows.UpArrow, laneDir * spawnStart, arrows.UpArrow.transform.rotation, transform);
            }
            else if (arrowIndex == 3)
            {
                obj = Instantiate(arrows.RightArrow, laneDir * spawnStart, arrows.RightArrow.transform.rotation, transform);
            }
        }
        else if (arrowIndex == 4)
        {
            aberrationIndex = UnityEngine.Random.Range(0, 4);
            GameObject m_abberation = aberration;

            if (aberrationIndex == 0)
            {
                obj = Instantiate(m_abberation, laneDir * spawnStart, arrows.DownArrow.transform.rotation, transform);
            }
            else if (aberrationIndex == 1)
            {
                obj = Instantiate(m_abberation, laneDir * spawnStart, arrows.LeftArrow.transform.rotation, transform);
            }
            else if (aberrationIndex == 2)
            {
                obj = Instantiate(m_abberation, laneDir * spawnStart, arrows.UpArrow.transform.rotation, transform);
            }
            else if (aberrationIndex == 3)
            {
                obj = Instantiate(m_abberation, laneDir * spawnStart, arrows.RightArrow.transform.rotation, transform);
            }
        }

        obj.GetComponent<ArrowMovement>().VectorDirection = moveDirection;

        if (aberrationIndex > -1)
        {
            obj.GetComponent<SpriteRenderer>().sprite = aberrationSprites[UnityEngine.Random.Range(0, aberrationSprites.Count - 1)];
            obj.GetComponent<SpriteRenderer>().color = arrowColors[UnityEngine.Random.Range(0, aberrationSprites.Count - 1)];
        }
    }

    int GetArrowIndex(int lane)
    {
        int randomizeTrigger = UnityEngine.Random.Range(0, 100);

        if (randomizeTrigger > 74)
        {
            return UnityEngine.Random.Range(0, 5);
        }
        else
        {
            return lane;
        }
    }
}
