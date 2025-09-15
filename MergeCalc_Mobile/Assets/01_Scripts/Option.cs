using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Option : MonoBehaviour
{
    public static Option Instance;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private GameObject blackView;

    [SerializeField] private Button continueButton, titleButton;
    private bool isOpen = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        gameObject.SetActive(false);
        blackView.SetActive(false);

        continueButton.clickEvent.AddListener(() => ButtonUseAble(false));
        titleButton.clickEvent.AddListener(() => ButtonUseAble(false));

        ButtonUseAble(false);
    }

    private void ButtonUseAble(bool _value)
    {
        continueButton.useAble = _value;
        titleButton.useAble = _value;
    }

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        gameObject.SetActive(true);
        blackView.SetActive(true);

        Time.timeScale = 0;
        StartCoroutine(CanvasGroupFade(true, () => ButtonUseAble(true)));
    }

    public void Close()
    {
        isOpen = false;
        Time.timeScale = 1;
        StartCoroutine(CanvasGroupFade(false, () => gameObject.SetActive(false)));
    }

    private IEnumerator CanvasGroupFade(bool _open, UnityAction _endAction)
    {
        canvas.alpha = _open ? 0 : 1;

        int _lastValue = _open ? 1 : 0;
        float _addValue = _open ? 0.1f : -0.1f;

        while (canvas.alpha != _lastValue)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            canvas.alpha += _addValue;
        }

        _endAction?.Invoke();
    }
}
