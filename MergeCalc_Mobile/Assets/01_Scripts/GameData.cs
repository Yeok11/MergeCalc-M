using UnityEngine;

public static class GameData
{
    public static int currentScore, highScore;

    public static void DataUpdate(int _score)
    {
        currentScore = _score;
        if(highScore < _score)
        {
            highScore = _score;
            PlayerPrefs.SetInt("Score4474", highScore);
        }
    }

    public static void Init()
    {
        highScore = PlayerPrefs.GetInt("Score4474");
    }
}