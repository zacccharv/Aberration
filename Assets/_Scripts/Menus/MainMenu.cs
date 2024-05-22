using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool _audioSelected;
    public List<AudioClip> Songs = new();
    private MenuScreens menuScreens;

    void Awake()
    {
        menuScreens = GetComponent<MenuScreens>();
    }

    public void PressPlay()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.gameState == GameState.Paused)
            {
                GameManager.Instance.ChangeGameState(GameState.Started);
                return;
            }
        }

        if (HighScores.Instance.scores.username == "" || HighScores.Instance.scores.username == null)
        {
            menuScreens.SwitchMenus(MenuType.Username);
            return;
        }
        else
        {
            // NOTE Sign In
            LeaderBoard.OnSignInAsync(HighScores.Instance.scores.username);
        }

        if (SceneManager.GetActiveScene().name != "Main")
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>().PlayMusic(1);
        }

        GetComponent<ButtonNavigation>().StopAllCoroutines();
        SceneManager.LoadScene("Assets/Scenes/Main.unity");
    }

    public void PressAudio()
    {
        menuScreens.SwitchMenus(MenuType.Audio);
    }

    public void PressScores()
    {
        // NOTE scores only if username exists 
        if (HighScores.Instance.scores.username != "")
        {
            LeaderBoard.OnSignInAsync("");
        }
        else
        {
            return;
        }

        menuScreens.SwitchMenus(MenuType.HighScores);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void PressMainMenu()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>().StopMusic();
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>().PlayMusic(0);
        }

        SceneManager.LoadScene("Assets/Scenes/MainMenu.unity");
    }
}
