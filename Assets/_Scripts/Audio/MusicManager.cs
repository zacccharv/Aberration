using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DontDestroyInstanced))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> Songs = new();
    public float musicVolume = 0, SFXVolume = 0;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(int index)
    {
        _audioSource.clip = Songs[index];
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}
