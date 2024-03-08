using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Started,
    Ended
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState;
    public float gameTime = 0;
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

        gameState = GameState.Started;
    }
    void Update()
    {
        gameTime += Time.deltaTime;
    }
}
