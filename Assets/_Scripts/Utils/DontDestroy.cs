using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    public static GameObject Instance;

    void Awake()
    {
        SingletonCheck();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += SingletonCheck;
    }

    private void SingletonCheck(Scene arg0, LoadSceneMode arg1)
    {
        if (GameObject.FindWithTag(tag) != gameObject && Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = gameObject;
        }
    }

    private void SingletonCheck()
    {
        if (GameObject.FindWithTag(tag) != gameObject && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = gameObject;
        }
    }
}