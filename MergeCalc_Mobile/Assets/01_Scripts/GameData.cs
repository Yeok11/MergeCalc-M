using UnityEngine;

public static class GameData
{
    #region Sound
    private static bool bgmMute = false, effectMute = false;

    public static void SoundInit()
    {
        bgmMute = bool.Parse(PlayerPrefs.GetString(AudioType.Bgm.ToString(), "false"));
        effectMute = bool.Parse(PlayerPrefs.GetString(AudioType.Effect.ToString(), "false"));
    }

    public static bool GetMuteValue(AudioType _type)
    {
        if (_type == AudioType.Bgm) return bgmMute;
        else return effectMute;
    }

    public static void SetMuteValue(AudioType _type, bool _value)
    {
        if (_type == AudioType.Bgm)
        {
            bgmMute = _value;
        }
        else if (_type == AudioType.Effect)
        {
            effectMute = _value;
        }

        PlayerPrefs.SetString(_type.ToString(), _value.ToString());
        PlayerPrefs.Save();
    }
    #endregion

    #region Score
    public static int currentScore { get; private set; } = 0;
    public static Mode currentMode;

    private const string scoreString = "HScore_";

    public static int GetHScore() => PlayerPrefs.GetInt(scoreString + currentMode);
    public static int GetHScore(Mode _m) => PlayerPrefs.GetInt(scoreString + _m);

    public static void ScoreUpdate(int _score, Mode _mode)
    {
        currentScore = _score;
        currentMode = _mode;

        if(GetHScore() < _score)
        {
            PlayerPrefs.SetInt(scoreString + _mode, _score);
            PlayerPrefs.Save();
        }
    }
    #endregion

    #region Drag
    public static bool canDrag;

    public static void InitDrag() => canDrag = true;
    #endregion
}