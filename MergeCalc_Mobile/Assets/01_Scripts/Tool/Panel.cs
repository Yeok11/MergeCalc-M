using UnityEngine;
using UnityEngine.Events;

public class Panel : MonoBehaviour
{
    [SerializeField] protected CanvasGroup canvas;
    [SerializeField] protected PanelType keyType;
    [SerializeField] protected float time = 0.1f;

    public UnityEvent onOpenEvent, onCloseEvent;
    public bool isOpenState { get; protected set; } = false;

    public PanelType GetKey() => keyType;
    public CanvasGroup GetCanvas() => canvas;

    public virtual void Init()
    {
        if (canvas == null) canvas = GetComponent<CanvasGroup>();

        canvas.gameObject.SetActive(false);
        onOpenEvent = new();
        onOpenEvent.AddListener(() => canvas.gameObject.SetActive(true));
        onCloseEvent = new();
        onCloseEvent.AddListener(() => canvas.gameObject.SetActive(false));
    }

    public virtual void Open()
    {
        if (isOpenState) return;
        isOpenState = true;

        onOpenEvent.Invoke();
        PanelFade.Instance.FadePanel(this, true, time, null);
    }
    
    public virtual void Close()
    {
        if (!isOpenState) return;
        isOpenState = false;

        PanelFade.Instance.FadePanel(this, false, time, onCloseEvent);
    }
}
