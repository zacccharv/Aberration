using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
using TMPro;
using System.IO;

public class LeaderBoard : MonoBehaviour
{
    // Create a leaderboard with this ID in the Unity Cloud Dashboard
    const string LeaderboardId = "Up_Down_Left_Right";
    [SerializeField] private string _user;
    [SerializeField] private Scores _personalScores;
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
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.GameStateChange -= GetScores;
        }
    }

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
            // Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };

        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            await AuthenticationService.Instance.UpdatePlayerNameAsync(string.Join("_", _user));
        }
    }

    public async void AddScore()
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, 100);

        // Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async void GetScores(GameState gameState)
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
            if (i < _personalScores.highScores.Count)
            {
                personalLBScores[i].text = _personalScores.highScores[_personalScores.highScores.Count - 1 - i].ToString();
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
            if (i < _personalScores.highScores.Count)
            {
                personalLBScores[i].text = _personalScores.highScores[_personalScores.highScores.Count - 1 - i].ToString();
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
            _personalScores = JsonUtility.FromJson<Scores>(json);

            if (_personalScores.highScores.Count <= 0) _personalScores.highScores.Add(0);
        }
        else
        {
            _personalScores.highScores.Add(0);

            File.WriteAllText(_path, JsonUtility.ToJson(_personalScores, true));
        }
    }
}
