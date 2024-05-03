using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Username : MonoBehaviour
{
    [SerializeField] private LeaderBoard _leaderBoard;

    public void CreateUsername(string input)
    {
        // NOTE Sign In

        LeaderBoard.OnSignIn(input);

        Invoke(nameof(Play), .2f);
    }

    public void Play()
    {
        if (SceneManager.GetActiveScene().name != "Main")
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>().PlayMusic(1);
        }

        SceneManager.LoadScene("Assets/Scenes/Main.unity");
    }

}
