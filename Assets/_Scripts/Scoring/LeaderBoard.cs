using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;

[RequireComponent(typeof(LogFile), typeof(DontDestroy))]
public class LeaderBoard : MonoBehaviour
{
    // Create a leaderboard with this ID in the Unity Cloud Dashboard
    public delegate Task LeaderboardSigninCallbackAsync(string name);
    public static event LeaderboardSigninCallbackAsync SignInAsync;

    public static LeaderBoard Instance;
    public string LeaderboardId = "Up_Down_Left_Right";
    public List<double> scores;
    public List<string> names;
    private LogFile logFile;

    void OnEnable()
    {
        SignInAsync += SignInAnonymouslyAsync;
    }

    void OnDisable()
    {
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

        if (Instance != this && Instance != null)
        {
            Destroy(Instance);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
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
