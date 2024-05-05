using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

[RequireComponent(typeof(LogFile))]
public class LeaderBoard : MonoBehaviour
{
    // Create a leaderboard with this ID in the Unity Cloud Dashboard
    public delegate Task LeaderboardSigninCallbackAsync(string name);
    public static event LeaderboardSigninCallbackAsync SignInAsync;

    const string LeaderboardId = "Up_Down_Left_Right";
    public List<double> scores;
    public List<string> names;
    public List<TextMeshProUGUI> leaderBoardScores, leaderBoardNames, personalLBScores;
    private LogFile logFile;

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
        logFile = GetComponent<LogFile>();
        HighScores.Instance.LoadScoreFile();

        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
            await SignInAnonymouslyAsync("default");
        }

    }
    void Start()
    {
        Invoke(nameof(GetScores), 1f);
    }

    /// <summary>
    /// Signs in on pressing play or pressing scores (if username exists)
    /// </summary>
    /// <param name="input">Username</param>
    /// <returns></returns>
    public async Task SignInAnonymouslyAsync(string input)
    {
        HighScores.Instance.LoadScoreFile();

        AuthenticationService.Instance.SignedIn += () =>
        {
            logFile.WriteToLog("PlayerId: " + AuthenticationService.Instance.PlayerId, LogLevel.Info);
        };

        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
            logFile.WriteToLog(s.ToString(), LogLevel.Error);
        };

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        // NOTE set new name
        if (!string.IsNullOrEmpty(HighScores.Instance.scores.username) && input != "default")
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(string.Join("_", HighScores.Instance.scores.username));
            logFile.WriteToLog($"Created Sign In {AuthenticationService.Instance.PlayerName}", LogLevel.Info);
        }

        logFile.WriteToLog($"Playername is {AuthenticationService.Instance.PlayerName}", LogLevel.Info);
    }
    public async void GetScores(GameState gameState)
    {
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);

        HighScores.Instance.LoadScoreFile();

        logFile.WriteToLog(JsonConvert.SerializeObject(scoresResponse), LogLevel.Info);

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
            if (i < HighScores.Instance.scores.highScores.Count)
            {
                personalLBScores[i].text = HighScores.Instance.scores.highScores[HighScores.Instance.scores.highScores.Count - 1 - i].ToString();
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

        HighScores.Instance.LoadScoreFile();

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
            if (i < HighScores.Instance.scores.highScores.Count)
            {
                personalLBScores[i].text = HighScores.Instance.scores.highScores[HighScores.Instance.scores.highScores.Count - 1 - i].ToString();
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

    public static void OnSignInAsync(string input)
    {
        // NOTE signs and creates online username if not added yet
        SignInAsync?.Invoke(input);
    }
}
