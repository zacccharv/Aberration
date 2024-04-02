using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public struct SequenceItem
{
    public Direction Lane;
    public GameObject ArrowPrefab;
}

[Serializable]
public struct Sequence
{
    public List<SequenceItem> SequenceItems;
}

[Serializable]
public struct SequencedSequences
{
    public List<Sequence> Sequences;
}

public class SpawnSequencing : MonoBehaviour
{
    public Queue<SequenceItem> arrowsToSpawn = new(100);
    public Vector2 spawnStart;
    public static int _stage;

    [SerializeField] private SequencedSequences _sequencedSequences;
    [SerializeField] private int _spawnCount;
    [SerializeField] private bool _test;
    [SerializeField] private int _testSequenceIndex;
    [SerializeField] private float _spawnInterval;

    private float _spawnTimer;

    // TODO different sequences for each stage

    #region Section 0
    // NOTE (Stages) 1st  stage single arrows only
    //  2rd  stage single arrows + double arrows + mixup direction lanes
    #endregion

    #region Section 1
    //  3rd stage single arrows + double arrows
    //  4th stage single arrows + double arrows + mixup lanes + Aberrations
    #endregion

    #region Section 2
    //  5th stage single arrows + double arrows + Long Arrows
    //  6th stage single arrows + double arrows + Long Arrows + mixuplanes 
    #endregion

    #region Section 3
    //  7th stage single arrows + double arrows + Long Arrows + mixup lanes + Aberrations
    //  after this stage repeat 7th stage each time with a chaos stage here and there
    #endregion

    void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer > _spawnInterval)
        {
            SpawnArrow();
            _spawnTimer = 0;
        }
    }

    public void AddSequence(int section)
    {
        Sequence sequence = _sequencedSequences.Sequences[section];

        int seqInd = _test ? _testSequenceIndex : UnityEngine.Random.Range(0, sequence.SequenceItems.Count);
        int count = sequence.SequenceItems.Count;

        for (int i = 0; i < count; i++)
        {
            arrowsToSpawn.Enqueue(sequence.SequenceItems[seqInd]);
        }
    }

    public GameObject DequeuePrefab(out float spawnInterval, out Direction lane)
    {
        SequenceItem result;

        if (arrowsToSpawn.Count < 1)
        {
            spawnInterval = 0;
            lane = 0;
            return null;
        }

        result = arrowsToSpawn.Dequeue();
        lane = result.Lane;

        spawnInterval = result.ArrowPrefab.GetComponent<Arrow>().spawnTime;

        return result.ArrowPrefab;
    }

    public void SpawnArrow()
    {
        _stage = Mathf.Min(_stage, _sequencedSequences.Sequences.Count - 1);

        if (arrowsToSpawn.Count < 1) AddSequence(_stage);
        _spawnCount++;

        GameObject go = DequeuePrefab(out _spawnInterval, out Direction lane);

        go.GetComponent<SortingGroup>().sortingOrder = _spawnCount;
        Vector2 laneDirection = Vector2.zero;

        // Change position depending on lane index
        switch (lane)
        {
            case Direction.Up:
                laneDirection = Vector2.up * spawnStart;
                break;
            case Direction.Right:
                laneDirection = Vector2.right * spawnStart;
                break;
            case Direction.Down:
                laneDirection = Vector2.down * spawnStart;
                break;
            case Direction.Left:
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
