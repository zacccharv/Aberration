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
    public GameState gameState;
    public GameObject menu;
    public float gameTime = 0;
    public static float deltaTime;
    public static float timeScale;

    void OnEnable()
    {
        InputManager_Z.UIInputPressed += EscCheck;
    }

    void OnDisable()
    {
        InputManager_Z.UIInputPressed -= EscCheck;
    }

    private void EscCheck(InputType inputType)
    {
        if (inputType == InputType.Esc)
        {
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>().StopMusic();
                GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>().PlayMusic(0);
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

        if (gameState == GameState.Ended || gameState == GameState.Paused)
            timeScale = 0;
        else
            timeScale = 1;

        menu.SetActive(true);
    }

}
