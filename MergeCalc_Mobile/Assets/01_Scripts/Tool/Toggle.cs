using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Toggle : Button
{
    [Header("Toggle")]
    [SerializeField] protected Image bgTarget;
    [SerializeField] private float offXValue = -111.9f;
    [SerializeField] protected Color onBGColor = Color.white, offBGColor = Color.white;

    public UnityEvent<bool> toggleEvent;

    protected Sequence btnSeq;
    protected bool value;

    protected override void Awake()
    {
        base.Awake();

        if (bgTarget == null) bgTarget = GetComponent<Image>();
    }

    protected virtual void Start()
    {
        btnTarget.transform.localPosition = new(value ? 0 : offXValue, 0, 0);
        bgTarget.color = value ? onBGColor : offBGColor;
    }

    protected override void ButtonAnime()
    {
        base.ButtonAnime();
        ToggleValue();
        ToggleAnime();
    }

    protected virtual void ToggleValue() => value = !value;

    protected virtual void ToggleAnime()
    {
        if (btnSeq != null && btnSeq.IsActive()) btnSeq.Kill(true);

        bgTarget.color = value ? onBGColor : offBGColor;

        btnSeq = DOTween.Sequence();
        btnSeq.SetUpdate(true);
        btnSeq.Append(btnTarget.transform.DOLocalMoveX(value ? 0 : offXValue, 0.1f));
        btnSeq.OnComplete(() => toggleEvent?.Invoke(value));
    }
}
