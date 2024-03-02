using System;
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

    public Arrows arrows;
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
        int lane = UnityEngine.Random.Range(0, 4);
        Debug.Log(lane);

        if (lane == 0)
        {
            Instantiate(arrows.DownArrow, spawnStart * Vector2.up, arrows.DownArrow.transform.rotation);
        }
        else if (lane == 1)
        {
            Instantiate(arrows.LeftArrow, spawnStart * Vector2.right, arrows.LeftArrow.transform.rotation);
        }
        else if (lane == 2)
        {
            Instantiate(arrows.UpArrow, spawnStart * Vector2.down, arrows.UpArrow.transform.rotation);
        }
        else if (lane == 3)
        {
            Instantiate(arrows.RightArrow, spawnStart * Vector2.left, arrows.RightArrow.transform.rotation);
        }
    }
}
