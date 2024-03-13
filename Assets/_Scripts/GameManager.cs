using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Started,
    Paused,
    Ended
}

public delegate void GameStateChange(GameState gameState);

public class GameManager : MonoBehaviour
{
    public static event GameStateChange GameStateChange;
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

    public void OnGameStateChange(GameState gameState)
    {
        this.gameState = gameState;
        GameStateChange?.Invoke(gameState);
    }
}
