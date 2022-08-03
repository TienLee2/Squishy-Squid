using UnityEngine;

public static class Score 
{
    public static void Start()
    {
        Squid.GetInstance().onDied += Level_onDied;
    }

    private static void Level_onDied(object sender, System.EventArgs e)
    {
        TrySetNewHighscore(Level.GetInstance().GetPipePassedCount());
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }

    public static bool TrySetNewHighscore(int score)
    {
        int currentHighScore = GetHighScore();
        if (score >= currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.Save();
    }
}
