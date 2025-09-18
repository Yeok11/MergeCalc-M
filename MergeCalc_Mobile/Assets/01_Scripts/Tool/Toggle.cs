using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : Button
{
    [Header("Toggle")]
    [SerializeField] protected Image bgTarget;
    [SerializeField] private float offXValue = -111.9f;
    [SerializeField] protected Color onBGColor = Color.white, offBGColor = Color.white;

    protected Sequence btnSeq;
    protected bool value;

    protected override void Awake()
    {
        base.Awake();

        if (bgTarget == null) bgTarget = GetComponent<Image>();
    }

    protected virtual void Start()
    {
        btnTarget.transform.localPosition = new(value ? offXValue : 0, 0, 0);
        bgTarget.color = value ? offBGColor : onBGColor;
    }

    protected override void ButtonAnime()
    {
        base.ButtonAnime();

        if (btnSeq != null && btnSeq.IsActive()) btnSeq.Kill(true);

        bgTarget.color = value ? offBGColor : onBGColor;

        btnSeq = DOTween.Sequence();
        btnSeq.SetUpdate(true);
        btnSeq.Append(btnTarget.transform.DOLocalMoveX(value ? offXValue : 0, 0.1f));
    }
}
