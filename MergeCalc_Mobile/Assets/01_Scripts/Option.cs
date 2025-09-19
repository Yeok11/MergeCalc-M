using UnityEngine;
using UnityEngine.Events;

public class Option : Panel
{
    [SerializeField] private bool timeScaleChange = true;

    [SerializeField] private Button[] buttons;

    private UnityAction<bool> buttonUseable;
    private bool isOpen = false;

    public override void Init()
    {
        base.Init();

        foreach (var _btn in buttons) 
            buttonUseable += (bool _value) => _btn.useAble = _value;

        foreach (var _btn in buttons) 
            _btn.clickEvent.AddListener(() => buttonUseable(false));

        onOpenEvent += () => buttonUseable?.Invoke(true);

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
        PlayerPrefs.Save();
    }
}
