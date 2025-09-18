using UnityEngine;

public class AudioToggle : Toggle
{
    [SerializeField] private AudioType type;

    protected override void Start()
    {
        value = !GameData.GetMuteValue(type);
        base.Start();

        toggleEvent.AddListener((bool _b) => SoundSystem.Instance.SoundStateChange(type, !_b));
        toggleEvent.AddListener((bool _b) => GameData.SetMuteValue(type, _b));
    }

    protected override void ButtonAnime()
    {
        base.ButtonAnime();
    }
}
