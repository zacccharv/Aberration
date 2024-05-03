using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
using TMPro;
using System.IO;
using Newtonsoft.Json;
using System;

public class LeaderBoard : MonoBehaviour
{
    // Create a leaderboard with this ID in the Unity Cloud Dashboard
    public delegate Task LeaderboardSigninCallbackAsync(string name);
    public delegate void LeaderboardSigninCallback(string name);
    public static event LeaderboardSigninCallbackAsync SignInAsync;
    public static event LeaderboardSigninCallback SignIn;

    const string LeaderboardId = "Up_Down_Left_Right";
    public Scores personalScores;
    public List<double> scores;
    public List<string> names;
    public List<TextMeshProUGUI> leaderBoardScores, leaderBoardNames, personalLBScores;

    string VersionId { get; set; }
    int Offset { get; set; }
    int Limit { get; set; }
    int RangeLimit { get; set; }
    List<string> FriendIds { get; set; }

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.GameStateChange += GetScores;
        }

        SignInAsync += SignInAnonymouslyAsync;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.GameStateChange -= GetScores;
        }

        SignInAsync -= SignInAnonymouslyAsync;
    }

    private async void Awake()
    {
        LoadScoreFile();

        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
            await SignInAnonymouslyAsync("");
        }

    }
    void Start()
    {
        Invoke(nameof(GetScores), .5f);
    }

    /// <summary>
    /// Signs in on pressing play or pressing scores (if username exists)
    /// </summary>
    /// <param name="input">Username</param>
    /// <returns></returns>
    public async Task SignInAnonymouslyAsync(string input)
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

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            if (string.IsNullOrEmpty(personalScores.username) && input != "")
            {
                // NOTE set new name
                await AuthenticationService.Instance.UpdatePlayerNameAsync(string.Join("_", input));

                Debug.Log($"Created Sign In {AuthenticationService.Instance.PlayerName}");
            }
        }
    }

    public async void GetScores(GameState gameState)
    {
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
        LoadScoreFile();

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

        for (int i = 0; i < personalLBScores.Count; i++)
        {
            if (i < personalScores.highScores.Count)
            {
                personalLBScores[i].text = personalScores.highScores[personalScores.highScores.Count - 1 - i].ToString();
            }
            else
            {
                personalLBScores[i].text = "000";
            }
        }

    }

    public async void GetScores()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
        LoadScoreFile();

        // Debug.Log(JsonConvert.SerializeObject(scoresResponse));

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

        for (int i = 0; i < personalLBScores.Count; i++)
        {
            if (i < personalScores.highScores.Count)
            {
                personalLBScores[i].text = personalScores.highScores[personalScores.highScores.Count - 1 - i].ToString();
            }
            else
            {
                personalLBScores[i].text = "000";
            }
        }

    }

    public async void GetPlayerScore()
    {
        var scoreResponse =
            await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
        // Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public void LoadScoreFile()
    {
        string _path = Application.dataPath + "/highScores-score.json";

        if (File.Exists(_path))
        {
            string json = File.ReadAllText(_path);
            personalScores = JsonUtility.FromJson<Scores>(json);

            if (personalScores.highScores.Count <= 0) personalScores.highScores.Add(0);
        }
        else
        {
            personalScores.highScores.Add(0);

            File.WriteAllText(_path, JsonUtility.ToJson(personalScores, true));
        }
    }

    public static void OnSignInAsync(string input)
    {
        // NOTE signs and creates online username if not added yet

        SignInAsync?.Invoke(input);
    }
}
