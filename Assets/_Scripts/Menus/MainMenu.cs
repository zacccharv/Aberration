using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneAsset _mainScene;
    public bool _audioSelected;

    public void PressPlay()
    {
        SceneManager.LoadScene(_mainScene.name);
    }

    public void PressAudio(Slider slider)
    {

        if (_audioSelected)
        {
            _audioSelected = false;
            return;
        }

        if (!_audioSelected)
        {
            slider.Select();
            _audioSelected = true;
        }
    }

    public void PressScore()
    {

    }
}
