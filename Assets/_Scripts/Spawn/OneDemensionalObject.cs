using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class OneDemensionalObject : MonoBehaviour
{
    public List<int> originalInts = new();
    public List<char> originalChars;
    public List<string> permutations = new();
    private List<int> CurrentInts = new();
    private List<int> ints = new();
    public int mod = 0;
    public int total = 0;
    void Awake()
    {
        if (ints.Count == 0)
        {
            for (var i = 0; i < originalInts.Count; i++)
            {
                ints.Add(0);
                CurrentInts.Add(0);
            }
        }

        HeapsAlgo(originalInts.Count, originalChars);
        permutations.Sort();

        // total = TotalPermutations();
    }

    void Update()
    {
        // if (mod == total) return;

        // AltPermute();
    }

    public void AltPermute()
    {
        for (var i = 0; i < ints.Count; i++)
        {
            ints[i] = (int)Mathf.Floor(mod / Mathf.Pow(ints.Count, i)) % ints.Count;
            CurrentInts[i] = originalInts[ints[i]];
        }

        CurrentInts.Reverse();
        permutations.Add($"{string.Join(", ", CurrentInts)}, mod = {mod}");

        mod++;
    }

    public int TotalPermutations()
    {
        return (int)Mathf.Pow(ints.Count, ints.Count);
    }

    public void HeapsAlgo<T>(int k, List<T> items)
    {
        if (k == 1)
        {
            permutations.Add(string.Join(", ", items));
            return;
        }
        else
        {
            for (int i = 0; i < k; i++)
            {
                HeapsAlgo(k - 1, items);

                if (k % 2 == 0)
                {
                    items = Swap(items, i, k - 1);
                }
                else
                {
                    items = Swap(items, 0, k - 1);
                }
            }
        }
    }
    public List<T> Swap<T>(List<T> items, int index_1, int index_2)
    {
        T a = items[index_1];
        T b = items[index_2];

        items[index_2] = a;
        items[index_1] = b;

        return items;
    }
}
