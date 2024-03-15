using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepMusic : MonoBehaviour
{
    public static KeepMusic Instance;
    private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> Songs = new();
    public float musicVolume = 0, SFXVolume = 0;

    void Awake()
    {
        SingletonCheck();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += SingletonCheck;

        _audioSource = GetComponent<AudioSource>();
    }

    private void SingletonCheck(Scene arg0, LoadSceneMode arg1)
    {
        if (Instance != this && Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    private void SingletonCheck()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
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
