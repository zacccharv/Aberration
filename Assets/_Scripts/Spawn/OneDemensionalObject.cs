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

    public void HeapsAlgo(int k, List<char> items)
    {
        if (k == 1)
        {
            permutations.Add(string.Join(", ", items));
            Debug.Log(string.Join(", ", items));
            return;
        }
        else
        {
            for (int i = 0; i < k; i++)
            {
                HeapsAlgo(k - 1, items);
                char a, b;

                if (k % 2 == 0)
                {
                    a = items[i]; b = items[k - 1];
                    items[k - 1] = a;
                    items[i] = b;
                }
                else
                {
                    a = items[0]; b = items[k - 1];
                    items[k - 1] = a;
                    items[0] = b;
                }
            }
        }
    }
}
