using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    Success,
    SuccessNone,
    Fail,
    QuietSuccess,
    ComboUp,
    ComboReset,
    PerfectSuccess
}

[RequireComponent(typeof(DontDestroy))]
public class SFXCollection : MonoBehaviour
{
    public static SFXCollection Instance;
    [SerializeField] AudioSource _audioSource;
    public List<AudioClip> PerfectSounds = new();
    public List<AudioClip> SuccessSounds = new();
    public List<AudioClip> SuccessNoneSounds = new();
    public List<AudioClip> FailSounds = new();
    public AudioClip Noise;
    public float initialVolume;
    public float lowerVolume;
    private float originalPitch;


    void Start()
    {
        Instance = this;
        originalPitch = _audioSource.pitch;
    }

    public void PlaySound(SFXType sound)
    {
        List<AudioClip> clips = new();

        _audioSource.volume = initialVolume;
        _audioSource.pitch = originalPitch;

        float pitch = _audioSource.pitch;

        if (sound == SFXType.Success)
        {
            clips = SuccessSounds;
        }
        else if (sound == SFXType.PerfectSuccess)
        {
            clips = PerfectSounds;
        }
        else if (sound == SFXType.SuccessNone)
        {
            clips = SuccessNoneSounds;
        }
        else if (sound == SFXType.Fail)
        {
            clips = FailSounds;
        }
        else if (sound == SFXType.QuietSuccess)
        {
            _audioSource.volume = lowerVolume;
            clips = SuccessSounds;
        }
        else if (sound == SFXType.ComboUp)
        {
            clips = SuccessSounds;
            _audioSource.pitch = pitch + .3f;
        }
        else if (sound == SFXType.ComboReset)
        {
            clips = SuccessSounds;
            _audioSource.pitch = pitch - .3f;
        }

        AudioClip clip = clips[Random.Range(0, clips.Count)];
        //Debug.Log($"{clip}, {_audioSource.gameObject.name}");

        _audioSource.PlayOneShot(clip, initialVolume);
    }
}
