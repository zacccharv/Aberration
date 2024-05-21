using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> Songs = new();
    public float musicVolume, SFXVolume, masterVolume;
    [SerializeField] bool isOriginal;

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(Instance);
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
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
