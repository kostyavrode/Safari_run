using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Manager : MonoBehaviour
{
    public int levelCount = 50;
    public Text coin = null;
    public Text distance = null;
    public Camera camera = null;
    public GameObject guiGameOver = null;
    public LevelGenerator levelGenerator = null;
    public Text bestScoreText;
    private int currentCoins = 0;
    private int currentDistance = 0;
    private bool canPlay = false;

    private static Manager s_Instance;
    public static Manager instance
    {
        get
        {
            if ( s_Instance == null )
            {
                s_Instance = FindObjectOfType ( typeof ( Manager ) ) as Manager;
            }

            return s_Instance;
        }
    }
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Money"))
        {
            PlayerPrefs.SetInt("Money", 0);
            PlayerPrefs.Save();
        }
        else
        {
            UpdateCoinCount(0);
        }
    }
    void Start ()
    {
        for ( int i = 0; i < levelCount; i++ )
        {
            levelGenerator.RandomGenerator ();
        }
    }

    public void UpdateCoinCount ( int value )
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + value);
        PlayerPrefs.Save();
        currentCoins += value;

        coin.text = PlayerPrefs.GetInt("Money").ToString ();
    }

    public void UpdateDistanceCount ()
    {
        Debug.Log ( "Player moved forward for one point" );

        currentDistance += 1;

        distance.text = currentDistance.ToString ();

        levelGenerator.RandomGenerator ();
    }

    public bool CanPlay ()
    {
        return canPlay;
    }
    public void StartPlay ()
    {
        canPlay = true;
    }

    public void GameOver ()
    {
        camera.GetComponent<CameraShake> ().Shake ();
        camera.GetComponent<CameraFollow> ().enabled = false;
        CheckBestScore();
        GuiGameOver ();
    }
    private void CheckBestScore()
    {
        if (PlayerPrefs.HasKey("Bestscore"))
        {
            if (currentDistance>PlayerPrefs.GetInt("Bestscore"))
            {
                PlayerPrefs.SetInt("Bestscore", currentDistance);
                PlayerPrefs.Save();
                bestScoreText.text = currentDistance.ToString();
            }
            else
            {
                bestScoreText.text = PlayerPrefs.GetInt("Bestscore").ToString();
            }
        }
        else
        {
            PlayerPrefs.SetInt("Bestscore", currentDistance);
            PlayerPrefs.Save();
            bestScoreText.text = currentDistance.ToString();
        }
    }

    void GuiGameOver ()
    {
        Debug.Log ( "Game over!" );

        guiGameOver.SetActive ( true );
    }

    public void PlayAgain ()
    {
        Scene scene = SceneManager.GetActiveScene ();

        SceneManager.LoadScene ( scene.name );
    }

    public void Quit ()
    {
        Application.Quit ();
    }
}
