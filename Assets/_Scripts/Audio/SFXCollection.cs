using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    Success,
    SuccessNone,
    Fail
}

[RequireComponent(typeof(DontDestroy))]
public class SFXCollection : MonoBehaviour
{
    public static SFXCollection Instance;
    [SerializeField] AudioSource _audioSource;
    public List<AudioClip> SuccessSounds = new();
    public List<AudioClip> SuccessNoneSounds = new();
    public List<AudioClip> FailSounds = new();

    void Start()
    {
        Instance = this;
    }

    public void PlaySound(SFXType sound)
    {
        List<AudioClip> clips = new();

        if (sound == SFXType.Success)
        {
            clips = SuccessSounds;
        }
        else if (sound == SFXType.SuccessNone)
        {
            clips = SuccessNoneSounds;
        }
        else if (sound == SFXType.Fail)
        {
            clips = FailSounds;
        }

        _audioSource.PlayOneShot(clips[Random.Range(0, clips.Count)]);
    }
}
