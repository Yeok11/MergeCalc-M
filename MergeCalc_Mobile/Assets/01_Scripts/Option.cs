using UnityEngine;
using UnityEngine.Events;

public class Option : MonoBehaviour
{
    public static Option Instance;

    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private GameObject blackView;
    [SerializeField] private bool timeScaleChange = true;

    [SerializeField] private Button[] buttons;

    private UnityAction<bool> buttonUseable;
    private bool isOpen = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        canvas.gameObject.SetActive(false);
        blackView.SetActive(false);

        foreach (var _btn in buttons)
        {
            buttonUseable += (bool _value) => _btn.useAble = _value;
        }

        foreach (var _btn in buttons)
        {
            _btn.clickEvent.AddListener(() => buttonUseable(false));
        }

        buttonUseable?.Invoke(false);
    }

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        canvas.gameObject.SetActive(true);
        blackView.SetActive(true);

        if (timeScaleChange) Time.timeScale = 0;
        CanvasFade.Instance.FadeCanvas(canvas, true, 0.5f, () =>
        {
            if (isOpen) buttonUseable?.Invoke(true); 
        });
    }

    public void Close()
    {
        isOpen = false;
        if (timeScaleChange) Time.timeScale = 1;

        buttonUseable?.Invoke(false);
        CanvasFade.Instance.FadeCanvas(canvas, false, 0.5f, () =>
        {
            if (isOpen) buttonUseable?.Invoke(false);
        });
    }

    private void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}
