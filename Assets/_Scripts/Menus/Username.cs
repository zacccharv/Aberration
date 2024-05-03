using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Username : MonoBehaviour
{
    [SerializeField] private LeaderBoard _leaderBoard;
    [SerializeField] private HighScores _highScores;
    private string _name;

    public void CreateUsername(string input)
    {
        // NOTE Sign In

        _highScores.AddName(input);
        _name = input;

        Invoke(nameof(SignInInvoke), .5f);
        Invoke(nameof(Play), 1f);
    }

    public void Play()
    {
        if (SceneManager.GetActiveScene().name != "Main")
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>().PlayMusic(1);
        }

        SceneManager.LoadScene("Assets/Scenes/Main.unity");
    }

    public void SignInInvoke()
    {
        LeaderBoard.OnSignInAsync(_name);
    }
}
