using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class VolumeSliders
{
    public float MasterVolume = 0, SFXVolume = 0, MusicVolume = -2;
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource _audioSource;
    public VolumeSliders volumeSliders;
    [SerializeField] private List<AudioClip> Songs = new();
    [SerializeField] bool isOriginal;
    private string _path;

    void OnEnable()
    {
        Application.quitting += WriteVolumeSliders;
#if UNITY_WEBGL
        GameManager.GameStateChange += WriteVolumeSliders;
#endif
    }
    void OnDisable()
    {
        Application.quitting -= WriteVolumeSliders;
#if UNITY_WEBGL
        GameManager.GameStateChange -= WriteVolumeSliders;
#endif
    }

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

        // LoadVolumeSliders(out volumeSliders);
        _path = Application.persistentDataPath + "/volume-sliders.json";

        UnityFileManipulation.LoadJsonFile(_path, out volumeSliders);
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

    public void LoadVolumeSliders(out VolumeSliders volumeSliders)
    {
        _path = Application.persistentDataPath + "/volume-sliders.json";

        if (File.Exists(_path) && File.ReadAllText(_path) != "")
        {
            string json = File.ReadAllText(_path);

            volumeSliders = JsonUtility.FromJson<VolumeSliders>(json);
        }
        else
        {
            volumeSliders = new();

            File.WriteAllText(_path, JsonUtility.ToJson(volumeSliders, true));
        }
    }

    public void WriteVolumeSliders()
    {
        _path = Application.persistentDataPath + "/volume-sliders.json";

        UnityFileManipulation.WriteJsonFile(_path, volumeSliders);
    }
    public void WriteVolumeSliders(GameState _)
    {
        _path = Application.persistentDataPath + "/volume-sliders.json";

        UnityFileManipulation.WriteJsonFile(_path, volumeSliders);
    }
}
