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
public struct ArrowStruct
{
    public Direction Lane;
    public Direction Arrow;
    public InteractionType Interaction;
}

[Serializable]
public struct Sequence
{
    public List<ArrowStruct> SequenceItems;
}

[Serializable]
public struct SequencesContainer
{
    public List<Sequence> Sequences;
}

[Serializable]
public struct SectionsContainer
{
    public List<SequencesContainer> Sections;
}

public class SpawnSequencing : MonoBehaviour
{
    // TODO implement arrowstruct sequences
    public Queue<ArrowStruct> arrowsToSpawn = new(100);
    public Vector2 spawnStart;
    public static int _stage;

    [SerializeField] private SectionsContainer _sectionContainers;
    [SerializeField] private int _spawnCount;
    [SerializeField] private bool _test;
    [SerializeField] private int _testSequenceIndex;
    [SerializeField] private float _spawnInterval;

    private float _spawnTimer;

    // TODO different sequences for each stage
    // NOTE Randomly rotate sequences each time

    #region Section 0
    // NOTE (Stages) 1st  stage single arrows only
    //  2rd  stage single arrows + mixup direction lanes
    #endregion

    #region Section 1
    //  3rd stage single arrows + double arrows
    //  4th stage single arrows + double arrows + mixup lanes
    #endregion

    #region Section 1.5
    //  4.5th stage single arrows + double arrows + mixup lanes + Aberrations
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
        SequencesContainer sequence = _sectionContainers.Sections[section];

        int seqInd = _test ? _testSequenceIndex : UnityEngine.Random.Range(0, sequence.Sequences.Count);
        int count = sequence.Sequences[seqInd].SequenceItems.Count;

        for (int i = 0; i < count; i++)
        {
            arrowsToSpawn.Enqueue(sequence.Sequences[seqInd].SequenceItems[i]);
        }
    }

    public GameObject DequeuePrefab(out float spawnInterval, out Vector2 laneStartPos)
    {
        ArrowStruct arrowStruct;
        Direction lane;
        GameObject result;

        if (arrowsToSpawn.Count < 1)
        {
            spawnInterval = 0;
            laneStartPos = default;

            return null;
        }

        arrowStruct = arrowsToSpawn.Dequeue();
        lane = arrowStruct.Lane;

        result = GetArrow(arrowStruct, lane);

        spawnInterval = result.GetComponent<Arrow>().spawnTime;
        result.GetComponent<SortingGroup>().sortingOrder = _spawnCount;

        // Change position depending on lane index
        laneStartPos = lane switch
        {
            Direction.Up => Vector2.up * spawnStart,
            Direction.Right => Vector2.right * spawnStart,
            Direction.Down => Vector2.down * spawnStart,
            Direction.Left => Vector2.left * spawnStart,
            _ => default,
        };

        return result;
    }

    public GameObject GetArrow(ArrowStruct arrowStruct, Direction lane)
    {
        ArrowPrefabs arrowPrefabs = default;

        if (arrowStruct.Interaction == InteractionType.Single)
        {
            arrowPrefabs = ArrowManager.Instance.arrows.Single;
        }
        else if (arrowStruct.Interaction == InteractionType.Double)
        {
            arrowPrefabs = ArrowManager.Instance.arrows.Double;
        }
        else if (arrowStruct.Interaction == InteractionType.Long)
        {
            arrowPrefabs = lane switch
            {
                Direction.Right => ArrowManager.Instance.arrows.HoldRightLane,
                Direction.Down => ArrowManager.Instance.arrows.HoldDownLane,
                Direction.Left => ArrowManager.Instance.arrows.HoldLeftLane,
                Direction.Up => ArrowManager.Instance.arrows.HoldUpLane,
                _ => throw new NotImplementedException(),
            };
        }

        GameObject result = arrowStruct.Arrow switch
        {
            Direction.Up => arrowPrefabs.UpArrow,
            Direction.Right => arrowPrefabs.RightArrow,
            Direction.Down => arrowPrefabs.DownArrow,
            Direction.Left => arrowPrefabs.LeftArrow,
            _ => throw new NotImplementedException(),
        };

        return result;
    }
    public void SpawnArrow()
    {
        _stage = Mathf.Min(_stage, _sectionContainers.Sections.Count - 1);
        if (arrowsToSpawn.Count < 1) AddSequence(_stage);

        GameObject go = DequeuePrefab(out _spawnInterval, out Vector2 laneSpawnPos);

        Instantiate(go, laneSpawnPos, go.transform.localRotation, transform);
        _spawnCount++;

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
