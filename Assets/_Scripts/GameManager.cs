using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    PreStart,
    Started,
    Paused,
    Ended
}

public delegate void GameStateChange(GameState gameState);

[RequireComponent(typeof(LogFile))]
public class GameManager : MonoBehaviour
{
    public static event GameStateChange GameStateChange;
    public static GameManager Instance;
    private MenuScreens menuScreens;
    public GameState gameState;
    private GameObject _menu, _gameOverTitle, _pauseTitle;
    public float gameTime = 0;
    public static float deltaTime;
    public static float timeScale;

    void OnEnable()
    {
        SceneManager.sceneLoaded += FindObjects;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= FindObjects;
    }

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(Instance);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        gameState = GameState.PreStart;
        timeScale = 0;
    }

    void Update()
    {
        if (gameState == GameState.PreStart)
            return;

        float previousGameTime = gameTime;

        gameTime += Time.deltaTime * timeScale;

        deltaTime = gameTime - previousGameTime;
    }

    // TODO implement custom time scale
    public void ChangeGameState(GameState gameState)
    {
        if (gameState == GameState.PreStart)
            return;

        this.gameState = gameState;
        GameStateChange?.Invoke(gameState);

        if (gameState == GameState.Ended)
        {
            menuScreens.SwitchMenus(MenuType.MainMenu);
            DOTween.KillAll();

            timeScale = 0;

            _menu.SetActive(true);
            _pauseTitle.SetActive(false);
            _gameOverTitle.SetActive(true);

        }
        // FIXME Pause title staying in Scores menu
        else if (gameState == GameState.Paused)
        {
            timeScale = 0;
            _menu.SetActive(true);

            _menu.SetActive(true);
            _pauseTitle.SetActive(true);
            _gameOverTitle.SetActive(false);
        }
        else
        {
            timeScale = 1;
            menuScreens.menuType = MenuType.None;

            _menu.SetActive(false);
            _pauseTitle.SetActive(false);
            _gameOverTitle.SetActive(false);
        }

    }

    public void FindObjects(Scene scene, LoadSceneMode sceneMode)
    {
        GetComponent<LogFile>().WriteToLog(scene + " Loaded", LogLevel.Debug);

        if (scene.name == "Main")
        {
            _menu = FindAnyObjectByType<CMenu>(FindObjectsInactive.Include).gameObject;
            _gameOverTitle = FindAnyObjectByType<CGameOverTitle>(FindObjectsInactive.Include).gameObject;
            _pauseTitle = FindAnyObjectByType<CPauseTitle>(FindObjectsInactive.Include).gameObject;

            menuScreens = FindFirstObjectByType<MenuScreens>();
            ChangeGameState(GameState.Started);
            gameTime = 0;
        }

    }
}
