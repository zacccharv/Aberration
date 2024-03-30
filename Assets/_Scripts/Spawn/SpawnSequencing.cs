using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public struct Sequence
{
    public List<GameObject> ArrowGameObjs;
}
public class SpawnSequencing : MonoBehaviour
{
    public Queue<GameObject> arrowsToSpawn = new(100);
    public Vector2 spawnStart;
    [SerializeField] private bool _test;
    [SerializeField] private int _testSequenceIndex;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private Sequence[] _sequences;
    private float _spawnTimer;
    [SerializeField] int _spawnCount;

    void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer > _spawnInterval)
        {
            SpawnArrow();
            _spawnTimer = 0;
        }
    }

    public void AddSequence()
    {
        int seqInd = _test ? _testSequenceIndex : UnityEngine.Random.Range(0, _sequences.Length);
        int count = _sequences[seqInd].ArrowGameObjs.Count;

        for (int i = 0; i < count; i++)
        {
            Debug.Log(i);
            arrowsToSpawn.Enqueue(_sequences[seqInd].ArrowGameObjs[i]);
        }
    }
    public GameObject DequeuePrefab(out float spawnInterval, out int lane)
    {
        GameObject result;

        if (arrowsToSpawn.Count < 1)
        {
            spawnInterval = 0;
            lane = 0;
            return null;
        }

        result = arrowsToSpawn.Dequeue();
        spawnInterval = result.GetComponent<Arrow>().spawnTime;
        lane = (int)result.GetComponent<Arrow>().direction;

        return result;
    }
    public void SpawnArrow()
    {
        if (arrowsToSpawn.Count < 1) AddSequence();
        _spawnCount++;

        // NOTE add random lanes after all arrows introduced
        // int lane = GetLaneIndex(_previousLanes);
        // _previousLanes.Add(lane);

        GameObject go = DequeuePrefab(out _spawnInterval, out int lane);
        go.GetComponent<SortingGroup>().sortingOrder = _spawnCount;
        Vector2 laneDirection = Vector2.zero;

        // Change position depending on lane index
        switch (lane)
        {
            case 0:
                laneDirection = Vector2.up * spawnStart;
                break;
            case 1:
                laneDirection = Vector2.right * spawnStart;
                break;
            case 2:
                laneDirection = Vector2.down * spawnStart;
                break;
            case 3:
                laneDirection = Vector2.left * spawnStart;
                break;
            default:
                break;
        }

        Instantiate(go, laneDirection, go.transform.localRotation, transform);

#pragma warning disable CS8321 // Local function is declared but never used
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
#pragma warning restore CS8321 // Local function is declared but never used
    }
}
