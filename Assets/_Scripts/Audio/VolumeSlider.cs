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
        if (_mixerName == "Music")
        {
            _slider.value = MusicManager.Instance.musicVolume;
        }
        else if (_mixerName == "SFX")
        {
            _slider.value = MusicManager.Instance.SFXVolume;
        }
    }

    private void Update()
    {
        _mixer.SetFloat(_mixerName, _slider.value);

        if (_mixerName == "Music")
        {
            MusicManager.Instance.musicVolume = _slider.value;
        }
        else if (_mixerName == "SFX")
        {
            MusicManager.Instance.SFXVolume = _slider.value;
        }
    }
}
