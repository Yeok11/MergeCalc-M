using UnityEngine;
using DG.Tweening;

public class AudioToggle : Toggle
{
    [SerializeField] private AudioType type;

    protected override void Start()
    {
        value = GameData.GetMuteValue(type);
        base.Start();
    }

    protected override void ButtonAnime()
    {
        value = !GameData.GetMuteValue(type);
        base.ButtonAnime();

        SoundSystem.Instance.SoundStateChange(type, value);
        btnSeq.OnComplete(() => GameData.SetMuteValue(type, value));
    }
}
