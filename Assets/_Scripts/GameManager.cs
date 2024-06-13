using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    PreStart,
    Started,
    Paused,
    Ended,
    Tutorial
}

public delegate void GameStateChange(GameState gameState);

[RequireComponent(typeof(LogFile))]
public class GameManager : MonoBehaviour
{
    public static event GameStateChange GameStateChange;
    public static GameManager Instance;
    private MenuScreens menuScreens;
    public GameState gameState;

    [HideInInspector] public GameObject menu, gameOverTitle, pauseTitle;
    [HideInInspector] public GameObject title;

    public float gameTime = 0;
    public static float deltaTime;
    public static float timeScale;
    public float speedExpand;
    public float actualGameTime;
    [SerializeField] private float _timeScaleOffset;

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

        title = FindAnyObjectByType<CPauseTitle>(FindObjectsInactive.Include).gameObject;
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
            timeScale = 0;
        }
        else if (gameState == GameState.Paused)
        {
            timeScale = 0;

            menu.SetActive(true);
            pauseTitle.SetActive(true);
            gameOverTitle.SetActive(false);

            title = pauseTitle;
        }
        else if (gameState == GameState.Tutorial)
        {
            timeScale = 0;
        }
        else
        {
            timeScale = GetTimeScale();
            menuScreens.menuType = MenuType.None;

            menu.SetActive(false);
            pauseTitle.SetActive(false);
            gameOverTitle.SetActive(false);

            title = pauseTitle;
        }

    }
    public float SetTimeScale(float maxScore)
    {
        // NOTE: speed up at 600 point interval and every 60 seconds, add together

        int scoreCalc = Mathf.FloorToInt(maxScore / 600);
        int timeCalc = Mathf.FloorToInt(Time.realtimeSinceStartup / 60);

        int result = scoreCalc + timeCalc;

        speedExpand = result * .0333f;
        Mathf.Clamp(speedExpand, .5f, speedExpand);

        timeScale = 1 + (speedExpand - _timeScaleOffset);
        return timeScale;
    }

    public float GetTimeScale()
    {
        return 1 + (speedExpand - _timeScaleOffset);
    }

    public void FindObjects(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == "Main")
        {
            menuScreens = FindFirstObjectByType<MenuScreens>();

            menu = FindAnyObjectByType<CMenu>(FindObjectsInactive.Include).gameObject;
            gameOverTitle = FindAnyObjectByType<CGameOverTitle>(FindObjectsInactive.Include).gameObject;
            pauseTitle = FindAnyObjectByType<CPauseTitle>(FindObjectsInactive.Include).gameObject;

            ChangeGameState(GameState.Started);

            gameTime = 0;
        }

    }

    public void EndMe()
    {
        DOTween.KillAll();
        menuScreens.SwitchMenus(MenuType.HighScores);

        menu.SetActive(true);
        pauseTitle.SetActive(false);
        gameOverTitle.SetActive(false);

        title = gameOverTitle;
    }
}
