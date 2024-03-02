using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public static Tower Instance;
    public Bounds bounds;
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
}
