using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private string _mixerName;

    void Start()
    {
        if (_mixerName == "Master")
        {
            _slider.value = MusicManager.Instance.masterVolume;
        }
        else if (_mixerName == "Music")
        {
            _slider.value = MusicManager.Instance.musicVolume;
        }
        else if (_mixerName == "SFX")
        {
            _slider.value = MusicManager.Instance.SFXVolume;
        }
    }

    public void SetVolume(float value)
    {
        _mixer.SetFloat(_mixerName, value);

        if (_mixerName == "Master")
        {
            MusicManager.Instance.masterVolume = value;
        }
        else if (_mixerName == "Music")
        {
            MusicManager.Instance.musicVolume = value;
        }
        else if (_mixerName == "SFX")
        {
            MusicManager.Instance.SFXVolume = value;
        }
    }
}
