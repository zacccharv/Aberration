using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreButton : MonoBehaviour
{
    void OnEnable()
    {
        HighScores.ScoresLoaded += OnScoresLoaded;
        MenuScreens.ScreenSwitch += OnScoresLoaded;
    }

    void OnDisable()
    {
        HighScores.ScoresLoaded -= OnScoresLoaded;
        MenuScreens.ScreenSwitch -= OnScoresLoaded;
    }

    private void OnScoresLoaded()
    {
        if (HighScores.Instance.scores.username == "" || HighScores.Instance.scores.username == null)
        {
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }
    }
}
