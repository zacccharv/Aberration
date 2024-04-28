using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool _audioSelected;
    public List<AudioClip> Songs = new();
    public MenuScreens menuScreens;

    public void PressPlay()
    {
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
        menuScreens.SwitchMenus(MenuType.HighScores);
    }

    public void PressBack()
    {
        menuScreens.SwitchMenus(MenuType.MainMenu);
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
