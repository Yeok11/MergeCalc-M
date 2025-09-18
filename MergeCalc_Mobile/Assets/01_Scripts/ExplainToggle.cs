using UnityEngine;

public class ExplainToggle : Toggle
{
    [SerializeField] private Mode mode;

    protected override void Start()
    {
        value = GameData.GetModeExplain(mode);
        Debug.Log(mode + " => " + value);
        base.Start();
        toggleEvent.AddListener((bool _v) => GameData.SetModeExplain(mode, _v));
    }
}
