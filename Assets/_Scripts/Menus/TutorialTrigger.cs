using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
public class TutorialsTriggered
{
    public bool stage_1, stage_2, stage_3, esc;

}

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private List<UnityEngine.Object> _videoClips = new();
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private GameObject _videoPlayerContainer;
    [SerializeField] private TextMeshProUGUI _press;
    [SerializeField] private UnityEngine.UI.Image _enterIcon;
    public TutorialsTriggered tutorialsTriggered;

    public void OnEnable()
    {
        InputManager_Z.UIInputPressed += StopVideo;
        ScoreManager.StageEvent += PlayVideo;
    }
    public void OnDisable()
    {
        InputManager_Z.UIInputPressed -= StopVideo;
        ScoreManager.StageEvent -= PlayVideo;
    }

    public void Awake()
    {
        string path = Application.persistentDataPath + "/tutorials.json";
        UnityFileManipulation.LoadJsonFile(path, out tutorialsTriggered);

        Invoke(nameof(InvokeEsc), .25f);
    }

    public void InvokeEsc()
    {
        PlayVideo("Esc Tutorial");
    }
    public void InvokeStage_1()
    {
        PlayVideo("Single Tutorial");
    }

    public void PlayVideo(string name)
    {
        if (name.Contains("Single") && tutorialsTriggered.stage_1 == true)
            return;
        else if (name.Contains("Double") && tutorialsTriggered.stage_2 == true)
            return;
        else if (name.Contains("Long") && tutorialsTriggered.stage_3 == true)
            return;
        else if (name.Contains("Esc") && tutorialsTriggered.esc)
        {
            if (tutorialsTriggered!.stage_1)
            {
                name = "Single Tutorial";
            }
            else
            {
                ScoreManager.Instance.startStages = true;
                return;
            }
        }

        GameManager.timeScale = 0;

        _videoPlayerContainer.SetActive(true);

        string videoPath = Application.streamingAssetsPath + "/" + name + ".mp4";

        if (name == "")
            throw new Exception($"VideoClip name is empty!");

        if (_videoClips.Exists((x) => x.name == name))
            _videoPlayer.url = videoPath;
        else
            throw new Exception($"{name} is not in list!");

        if (!_videoPlayer.isLooping)
            _videoPlayer.isLooping = true;

        _videoPlayer.Play();

        _videoPlayer.gameObject.GetComponent<RectTransform>().DOScale(1, .3f).SetEase(Ease.OutBack);
        _videoPlayerContainer.GetComponent<UnityEngine.UI.Image>().DOFade(.40f, .3f).SetEase(Ease.OutSine);

        _press.DOFade(1, .3f).SetEase(Ease.OutSine);
        _enterIcon.DOFade(1, .3f).SetEase(Ease.OutSine);
    }
    public void StopVideo(InputType inputType)
    {
        if (inputType == InputType.Confirm && _videoPlayer.isPlaying)
        {
            if (_videoPlayer.url.Contains("Single"))
            {
                tutorialsTriggered.stage_1 = true;
                ScoreManager.Instance.startStages = true;
            }
            else if (_videoPlayer.url.Contains("Double"))
                tutorialsTriggered.stage_2 = true;
            else if (_videoPlayer.url.Contains("Long"))
                tutorialsTriggered.stage_3 = true;
            else if (_videoPlayer.url.Contains("Esc"))
            {
                tutorialsTriggered.esc = true;
                Invoke(nameof(InvokeStage_1), .25f);
            }

            GameManager.timeScale = GameManager.Instance.GetTimeScale();
            _videoPlayer.Stop();

            _videoPlayer.gameObject.GetComponent<RectTransform>().localScale = new(1, 1);
            _videoPlayerContainer.GetComponent<UnityEngine.UI.Image>().DOFade(0, 0);

            _press.DOFade(.80f, 0);
            _enterIcon.DOFade(.80f, 0);

            UnityFileManipulation.WriteJsonFile(Application.persistentDataPath + "/tutorials.json", tutorialsTriggered);

            _videoPlayerContainer.SetActive(false);
        }
    }

}