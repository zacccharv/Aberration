using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public GameObject menu;
    public float gameTime = 0;

    void OnEnable()
    {
        InputMan.UIInputPressed += EscCheck;
    }

    void OnDrawGizmos()
    {
        InputMan.UIInputPressed -= EscCheck;
    }

    private void EscCheck(InputType inputType)
    {
        if (inputType == InputType.Esc)
        {
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                GameObject.FindGameObjectWithTag("Music").GetComponent<KeepMusic>().StopMusic();
                GameObject.FindGameObjectWithTag("Music").GetComponent<KeepMusic>().PlayMusic(0);
            }

            SceneManager.LoadScene("Assets/Scenes/MainMenu.unity");
        }
    }

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

        menu.SetActive(true);
    }

}
