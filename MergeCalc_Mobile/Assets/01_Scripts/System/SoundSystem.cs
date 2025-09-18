using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem Instance;

    [SerializeField] private List<SoundData> effectSoundData;
    private Dictionary<EffectAudioType, AudioClip> effectSoundDic;

    [SerializeField] private AudioSource bgmSource, effectSource;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else 
            Destroy(gameObject);
    }

    private void Start()
    {
        effectSource.clip = null;
        effectSoundDic = new();

        foreach (var _item in effectSoundData)
        {
            if (effectSoundDic.ContainsKey(_item.audioType))
                Debug.LogError($"This key is already contained in this Dictionary. Key -> {_item.audioType}");
            else
                effectSoundDic.Add(_item.audioType, _item.clip);
        }

        GameData.SoundInit();
        PlayBgm();
    }

    public void SoundStateChange(AudioType _type, bool _mute)
    {
        switch (_type)
        {
            case AudioType.Bgm:
                if (_mute) 
                    bgmSource.Pause();
                else 
                    PlayBgm();
                break;

            case AudioType.Effect:
                if (_mute) 
                    effectSource.Stop();
                break;
        }
    }

    public void UseEffectSound(EffectAudioType _type)
    {
        if (GameData.GetMuteValue(AudioType.Effect)) return;

        effectSource.PlayOneShot(effectSoundDic[_type]);
    }

    private void PlayBgm()
    {
        if (GameData.GetMuteValue(AudioType.Bgm)) return;
        bgmSource.Play();
    }
}
