using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public class LeaderBoardMenu : MonoBehaviour
{

    public LeaderboardScoresPage scoresResponse;
    public List<TextMeshProUGUI> leaderBoardScores, leaderBoardNames, personalLBScores;
    public TextMeshProUGUI playerScoreName;

    void OnEnable()
    {
        HighScores.ScoresLoaded += GetScores;
    }

    void OnDisable()
    {
        HighScores.ScoresLoaded -= GetScores;
    }

    void Start()
    {
        Invoke(nameof(GetScores), 1f);
    }

    public async void GetScores()
    {
        if (AuthenticationService.Instance.AccessToken == null)
            return;

        scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderBoard.Instance.LeaderboardId);

        Invoke(nameof(HurryUpAndWait), .2f);
    }
    void HurryUpAndWait()
    {
        HighScores.Instance.LoadScoreFile();
        LeaderBoard.Instance.scores.Clear();
        LeaderBoard.Instance.names.Clear();

        for (int i = 0; i < scoresResponse.Results.Count; i++)
        {
            LeaderBoard.Instance.scores.Add(scoresResponse.Results[i].Score);

            string name = scoresResponse.Results[i].PlayerName;
            name = name.Remove(name.ToString().Length - 5, 5);

            LeaderBoard.Instance.names.Add(name);
        }

        for (int i = 0; i < leaderBoardScores.Count; i++)
        {
            if (i < LeaderBoard.Instance.scores.Count)
            {
                leaderBoardScores[i].text = LeaderBoard.Instance.scores[i].ToString();
                leaderBoardNames[i].text = LeaderBoard.Instance.names[i];
            }
            else
            {
                leaderBoardScores[i].text = "000";
                leaderBoardNames[i].text = "Nobody";
            }
        }

        for (int i = 0; i < personalLBScores.Count; i++)
        {
            if (i < HighScores.Instance.scores.highScores.Count)
            {
                personalLBScores[i].text = HighScores.Instance.scores.highScores[HighScores.Instance.scores.highScores.Count - 1 - i].ToString();
            }
            else
            {
                personalLBScores[i].text = "000";
            }
        }

        playerScoreName.text = HighScores.Instance.scores.username;
    }
}
