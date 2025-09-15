using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image target;
    [SerializeField] private Color baseColor = Color.white, pressedColor = Color.gray;
    [SerializeField] private float fadeDuration = 0.1f;

    public UnityEvent clickEvent;
    public bool useAble = true;

    private void Awake()
    {
        if (target == null) target = GetComponent<Image>();

        InitButtonUi();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!useAble) return;
        target.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!useAble) return;

        clickEvent?.Invoke();
        Invoke("InitButtonUi", fadeDuration);
    }

    private void InitButtonUi()
    {
        target.color = baseColor;
    }
}
