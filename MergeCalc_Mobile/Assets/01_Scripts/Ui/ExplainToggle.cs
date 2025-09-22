using UnityEngine;

public class ExplainToggle : Toggle
{
    [SerializeField] private Mode mode;

    protected override void Start()
    {
        value = GameData.GetModeExplain(mode);
        base.Start();
        toggleEvent.AddListener((bool _v) => GameData.SetModeExplain(mode, _v));
    }
}
