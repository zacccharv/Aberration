using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Username : MonoBehaviour
{
    [SerializeField] private LeaderBoard _leaderBoard;
    [SerializeField] private HighScores _highScores;
    private string _name;

    public void CreateUsername(string input)
    {
        // NOTE Sign In

        if (input == "")
        {
            GetComponent<TMP_InputField>().text = "";

            ColorUtility.TryParseHtmlString("#FF5F6B", out Color color);
            (GetComponent<TMP_InputField>().placeholder as TextMeshProUGUI).color = color;
            (GetComponent<TMP_InputField>().placeholder as TextMeshProUGUI).text = "Nothing entered, Try Again!";

            GetComponent<TMP_InputField>().ActivateInputField();
            _highScores.AddName(input);
            _name = input;
        }
        else if (ProfanityFilter.Instance.ContainsProfanity(input))
        {
            GetComponent<TMP_InputField>().text = "";

            ColorUtility.TryParseHtmlString("#FF5F6B", out Color color);
            (GetComponent<TMP_InputField>().placeholder as TextMeshProUGUI).color = color;
            (GetComponent<TMP_InputField>().placeholder as TextMeshProUGUI).text = "Inappropriate username, Try Again!";

            GetComponent<TMP_InputField>().ActivateInputField();
            return;
        }
        else
        {
            _highScores.AddName(input);
            _name = input;
        }

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
