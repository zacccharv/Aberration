using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Username : MonoBehaviour
{
    [SerializeField] private LeaderBoard _leaderBoard;
    [SerializeField] private HighScores _highScores;
    [SerializeField] private TextMeshProUGUI press;
    [SerializeField] private Image image;
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

            return;
        }
        else if (input.Length > 8)
        {
            GetComponent<TMP_InputField>().text = "";

            ColorUtility.TryParseHtmlString("#FF5F6B", out Color color);
            (GetComponent<TMP_InputField>().placeholder as TextMeshProUGUI).color = color;
            (GetComponent<TMP_InputField>().placeholder as TextMeshProUGUI).text = "Can't be longer than 8 characters.";

            GetComponent<TMP_InputField>().ActivateInputField();

            return;
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

    public void DisableConfirmCheck(string input)
    {
        if (input.Length > 0)
        {
            press.alpha = 1.00f;
            image.color = new(image.color.r, image.color.g, image.color.b, 1.00f);
        }
        else
        {
            press.alpha = 0.47f;
            image.color = new(image.color.r, image.color.g, image.color.b, 0.47f);
        }
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
