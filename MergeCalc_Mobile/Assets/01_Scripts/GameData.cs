using UnityEngine;

public static class GameData
{
    public static bool canDrag;
    public static int currentScore;
    public static Mode currentMode;

    private const string scoreString = "HScore_";

    public static int GetHScore() => PlayerPrefs.GetInt(scoreString + currentMode);
    public static int GetHScore(Mode _m) => PlayerPrefs.GetInt(scoreString + _m);

    public static void DataUpdate(int _score, Mode _mode)
    {
        currentScore = _score;
        currentMode = _mode;

        if(GetHScore() < _score)
        {
            PlayerPrefs.SetInt(scoreString + _mode, _score);
        }
    }

    public static void Init()
    {
        canDrag = true;
    }
}