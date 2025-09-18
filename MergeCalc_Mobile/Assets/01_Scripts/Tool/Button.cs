using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Button")]
    [SerializeField] protected Image btnTarget;
    [SerializeField] protected Color baseColor = Color.white, pressedColor = Color.gray;
    [SerializeField] protected float fadeDuration = 0.1f;

    public UnityEvent clickEvent;
    [SerializeField] private bool useCoroutine;
    public bool useAble = true;

    protected virtual void Awake()
    {
        if (btnTarget == null) btnTarget = GetComponent<Image>();

        InitButtonUi();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!useAble) return;
        ButtonAnime();
    }

    protected virtual void ButtonAnime()
    {
        btnTarget.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!useAble) return;

        clickEvent?.Invoke();
        if (useCoroutine) 
            StartCoroutine(FadeDelay());
        else
            Invoke("InitButtonUi", fadeDuration);
    }

    private IEnumerator FadeDelay()
    {
        yield return new WaitForSecondsRealtime(fadeDuration);
        InitButtonUi();
    }

    private void InitButtonUi()
    {
        btnTarget.color = baseColor;
    }
}
