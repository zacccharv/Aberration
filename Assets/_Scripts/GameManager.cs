using DG.Tweening;
using UnityEngine;
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
    [SerializeField] private MenuScreens menuScreens;
    public GameState gameState;
    public GameObject menu, gameOverTitle, pauseTitle;
    public float gameTime = 0;
    public static float deltaTime;
    public static float timeScale;

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

        gameState = GameState.Started;
        timeScale = 1;
    }
    void Update()
    {
        float previousGameTime = gameTime;

        gameTime += Time.deltaTime * timeScale;

        deltaTime = gameTime - previousGameTime;
    }

    // TODO implement custom time scale
    public void ChangeGameStateChange(GameState gameState)
    {
        this.gameState = gameState;
        GameStateChange?.Invoke(gameState);

        if (gameState == GameState.Ended)
        {
            timeScale = 0;
            DOTween.KillAll();
            menu.SetActive(true);
            menuScreens.SwitchMenus(MenuType.MainMenu);

            pauseTitle.SetActive(false);
            gameOverTitle.SetActive(true);

        }
        else if (gameState == GameState.Paused)
        {
            timeScale = 0;
            menu.SetActive(true);

            pauseTitle.SetActive(true);
            gameOverTitle.SetActive(false);
        }
        else
        {
            timeScale = 1;
            menu.SetActive(false);

            pauseTitle.SetActive(false);
            gameOverTitle.SetActive(false);
        }

    }

}
