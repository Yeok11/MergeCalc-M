using UnityEngine;
using UnityEngine.Events;

public class OptionPanel : Panel
{
    [SerializeField] private bool timeScaleChange = true;

    [SerializeField] private Button[] buttons;

    private UnityEvent<bool> buttonUseable;
    private bool isOpen = false;

    public override void Init()
    {
        base.Init();

        buttonUseable = new();

        foreach (var _btn in buttons) 
            buttonUseable.AddListener((bool _value) => _btn.useAble = _value);

        foreach (var _btn in buttons) 
            _btn.clickEvent.AddListener(() => buttonUseable?.Invoke(false));

        onOpenEvent.AddListener(() => buttonUseable?.Invoke(true));

        buttonUseable?.Invoke(false);
    }

    public override void Open()
    {
        if (isOpen) return;

        if (timeScaleChange) Time.timeScale = 0;
        base.Open();
    }

    public override void Close()
    {
        buttonUseable?.Invoke(false);
        if (timeScaleChange) Time.timeScale = 1;
        base.Close();
    }

    private void OnDestroy()
    {
        foreach (var _btn in buttons)
            _btn.clickEvent.RemoveAllListeners();
        buttonUseable.RemoveAllListeners();
        PlayerPrefs.Save();
    }
}
