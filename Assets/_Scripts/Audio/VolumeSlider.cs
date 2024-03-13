using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private string _mixerName;

    private void Update()
    {
        _mixer.SetFloat(_mixerName, GetVolume(_slider.value));
    }

    private float GetVolume(float value)
    {
        if (value == 0)
        {
            return -80;
        }
        else
        {
            return -15 + (value * 15);
        }
    }
}
