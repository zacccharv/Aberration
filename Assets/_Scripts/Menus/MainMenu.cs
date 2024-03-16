using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public bool _audioSelected;
    public List<AudioClip> Songs = new();

    public void PressPlay()
    {
        if (SceneManager.GetActiveScene().name != "Main")
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<KeepMusic>().PlayMusic(1);
        }

        SceneManager.LoadScene("Assets/Scenes/Main.unity");
    }

    public void PressAudio(Slider slider)
    {

    }

    public void PressMainMenu()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<KeepMusic>().StopMusic();
            GameObject.FindGameObjectWithTag("Music").GetComponent<KeepMusic>().PlayMusic(0);
        }

        SceneManager.LoadScene("Assets/Scenes/MainMenu.unity");
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
