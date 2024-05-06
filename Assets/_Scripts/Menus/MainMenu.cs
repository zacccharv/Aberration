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
        if (menuScreens.previousMenuType != menuScreens.menuType)
        {
            // deals with race conditions
            menuScreens.previousMenuType = menuScreens.menuType;
            return;
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
        if (HighScores.Instance.scores.username != "")
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
