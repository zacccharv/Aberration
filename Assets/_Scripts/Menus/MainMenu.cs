using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool _audioSelected;
    public List<AudioClip> Songs = new();
    private MenuScreens menuScreens;
    [SerializeField] private LeaderBoard _leaderBoard;

    void Awake()
    {
        menuScreens = GetComponent<MenuScreens>();
    }

    public void PressPlay()
    {
        if (menuScreens.previousMenuType != menuScreens.menuType)
        {
            // deals with race conditions
            menuScreens.previousMenuType = menuScreens.menuType;
            return;
        }

        if (_leaderBoard.personalScores.username == "" || _leaderBoard.personalScores.username == null)
        {
            menuScreens.SwitchMenus(MenuType.Username);
            return;
        }
        else
        {
            Debug.Log($"{_leaderBoard.personalScores.username}");

            // NOTE Sign In
            LeaderBoard.OnSignInAsync(_leaderBoard.personalScores.username);
        }

        if (SceneManager.GetActiveScene().name != "Main")
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>().PlayMusic(1);
        }

        SceneManager.LoadScene("Assets/Scenes/Main.unity");
    }

    public void PressAudio()
    {
        menuScreens.SwitchMenus(MenuType.Audio);
    }

    public void PressScores()
    {
        Debug.Log("Score Pressed");

        // NOTE scores only if username exists 
        if (_leaderBoard.personalScores.username != "")
        {
            LeaderBoard.OnSignInAsync("");
        }
        else
        {
            Debug.Log("Play the game first");
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
