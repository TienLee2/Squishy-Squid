using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using CodeMonkey;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public GameObject gameOver;

    private void Awake()
    {
        Hide();
    }

    private void Start()
    {
        Squid.GetInstance().onDied += Level_onDied;
    }

    public void Update()
    {
        if (gameOver.activeSelf)
        {
            if (Input.GetKeyDown("space")|| Input.GetMouseButtonDown(0))
            {
                ReloadScene();
            }
        }
    }

    public void ReloadScene()
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    private void Level_onDied(object sender, System.EventArgs e)
    {
        scoreText.text = Level.GetInstance().GetPipePassedCount().ToString();
        if (Level.GetInstance().GetPipePassedCount() > Score.GetHighScore())
        {
            highscoreText.text = "NEW HIGHSCORE!";
        }
        else
        {
            highscoreText.text = "Highscore: " + Score.GetHighScore();
        }
        Invoke("Show", 2f);
    }

    private void Hide()
    {
        gameOver.SetActive(false);
    }

    private void Show()
    {
        gameOver.SetActive(true);
    }
}
