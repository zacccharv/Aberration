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

[Serializable]
public struct Aberrations
{
    public GameObject DownAbberation;
    public GameObject LeftAbberation;
    public GameObject UpAbberation;
    public GameObject RightAbberation;
}

public class LaneManager : MonoBehaviour
{
    public static LaneManager Instance;

    public static event Action MoveArrows;

    public Arrows arrows;
    public Aberrations aberrations;
    public List<Sprite> aberrationSprites;

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
        Vector2 direction = Vector2.zero;
        Vector2 laneDir;

        if (lane == 0)
        {
            direction = Vector2.down;
            GameObject gameObject = PickArrow(Vector2.down, out laneDir);
            Instantiate(gameObject, laneDir, arrows.DownArrow.transform.rotation);
        }
        else if (lane == 1)
        {
            direction = Vector2.right;
            GameObject gameObject = PickArrow(Vector2.right, out laneDir);
            Instantiate(gameObject, laneDir, arrows.LeftArrow.transform.rotation);
        }
        else if (lane == 2)
        {
            direction = Vector2.up;
            GameObject gameObject = PickArrow(Vector2.up, out laneDir);
            Instantiate(gameObject, laneDir, arrows.UpArrow.transform.rotation);
        }
        else if (lane == 3)
        {
            direction = Vector2.left;
            GameObject gameObject = PickArrow(Vector2.left, out laneDir);
            Instantiate(gameObject, laneDir, arrows.RightArrow.transform.rotation);
        }

        Debug.Log($"{lane}, {direction}");

        GameObject PickArrow(Vector2 direction, out Vector2 lanePos)
        {
            GameObject obj = default;
            int index = UnityEngine.Random.Range(0, 5);
            int aberrationIndex;
            lanePos = default;

            if (index < 4)
            {
                if (index == 0)
                {
                    obj = arrows.DownArrow;
                }
                else if (index == 1)
                {
                    obj = arrows.LeftArrow;
                }
                else if (index == 2)
                {
                    obj = arrows.UpArrow;
                }
                else if (index == 3)
                {
                    obj = arrows.RightArrow;
                }

                PrefabUtility.UnpackPrefabInstance(obj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            }
            else if (index == 4)
            {
                aberrationIndex = UnityEngine.Random.Range(0, 4);

                if (aberrationIndex == 0)
                {
                    obj = aberrations.DownAbberation;
                }
                else if (aberrationIndex == 1)
                {
                    obj = aberrations.LeftAbberation;
                }
                else if (aberrationIndex == 2)
                {
                    obj = aberrations.UpAbberation;
                }
                else if (aberrationIndex == 3)
                {
                    obj = aberrations.RightAbberation;
                }

                PrefabUtility.UnpackPrefabInstance(obj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);


                obj.GetComponent<SpriteRenderer>().sprite = aberrationSprites[UnityEngine.Random.Range(0, 4)];
            }

            obj.GetComponent<ArrowMovement>().VectorDirection = direction;
            obj.GetComponent<ArrowMovement>()._vecDirTest = direction;

            if (direction == Vector2.down)
            {
                lanePos = new(0, 5);
            }
            else if (direction == Vector2.left)
            {
                lanePos = new(5, 0);
            }
            else if (direction == Vector2.up)
            {
                lanePos = new(0, -5);
            }
            else if (direction == Vector2.right)
            {
                lanePos = new(-5, 0);
            }

            return obj;
        }
    }
}
