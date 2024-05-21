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

public class SFXCollection : MonoBehaviour
{
    public static SFXCollection Instance;
    [SerializeField] AudioSource _audioSource;
    public List<AudioClip> ComboUpSounds = new();
    public List<AudioClip> SuccessSounds = new();
    public List<AudioClip> SuccessNoneSounds = new();
    public List<AudioClip> FailSounds = new();
    public AudioClip Noise, ComboResetSound;
    public float initialVolume;
    public float lowerVolume;

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(Instance);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySound(SFXType sound)
    {
        List<AudioClip> clips = new();
        AudioClip clip;

        _audioSource.volume = initialVolume;

        if (sound == SFXType.Success)
        {
            clips = SuccessSounds;
        }
        else if (sound == SFXType.PerfectSuccess)
        {
            clips = ComboUpSounds;
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
            clips = ComboUpSounds;
        }
        else if (sound == SFXType.ComboReset)
        {
            clip = ComboResetSound;
            _audioSource.PlayOneShot(clip, initialVolume);

            return;
        }

        clip = clips[Random.Range(0, clips.Count)];
        //Debug.Log($"{clip}, {_audioSource.gameObject.name}");

        _audioSource.PlayOneShot(clip, initialVolume);
    }
}
