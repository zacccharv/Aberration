using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    // Create a leaderboard with this ID in the Unity Cloud Dashboard
    const string LeaderboardId = "Up_Down_Left_Right";
    [SerializeField] private string _user;
    public List<double> scores;
    public List<string> names;
    public List<TextMeshProUGUI> leaderBoardScores, leaderBoardNames;

    string VersionId { get; set; }
    int Offset { get; set; }
    int Limit { get; set; }
    int RangeLimit { get; set; }
    List<string> FriendIds { get; set; }

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await SignInAnonymouslyAsync();
    }

    void Start()
    {
        Invoke("AddScore", 2.0f);
        Invoke("GetScores", 3.0f);
        Invoke("GetPlayerScore", 4.0f);
    }

    private async Task SignInAnonymouslyAsync()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };

        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await AuthenticationService.Instance.UpdatePlayerNameAsync(string.Join("_", _user));
    }

    public async void AddScore()
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, 100);

        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetScores()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));

        for (int i = 0; i < scoresResponse.Results.Count; i++)
        {
            scores.Add(scoresResponse.Results[i].Score);

            string name = scoresResponse.Results[i].PlayerName;
            name = name.Remove(name.ToString().Length - 5, 5);

            names.Add(name);
        }

        for (int i = 0; i < leaderBoardScores.Count; i++)
        {
            if (i < scores.Count)
            {
                leaderBoardScores[i].text = scores[i].ToString();
                leaderBoardNames[i].text = names[i];
            }
            else
            {
                leaderBoardScores[i].text = "000";
                leaderBoardNames[i].text = "Nobody";
            }
        }
    }

    public async void GetPlayerScore()
    {
        var scoreResponse =
            await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }
}
