using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Option : MonoBehaviour
{
    public static Option Instance;

    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private GameObject blackView;

    [Header("Normal Button")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button giveUpButton;

    private UnityAction<bool> buttonUseable;
    private bool isOpen = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        canvas.gameObject.SetActive(false);
        blackView.SetActive(false);

        if (continueButton != null) buttonUseable += (bool _value) => continueButton.useAble = _value;
        if (giveUpButton != null) buttonUseable += (bool _value) => giveUpButton.useAble = _value;

        if (continueButton != null) continueButton.clickEvent.AddListener(() => buttonUseable(false));
        if (giveUpButton != null) giveUpButton.clickEvent.AddListener(() => buttonUseable(false));

        buttonUseable?.Invoke(false);
    }

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        canvas.gameObject.SetActive(true);
        blackView.SetActive(true);

        Time.timeScale = 0;
        CanvasFade.Instance.FadeCanvas(canvas, true, 0.5f, () =>
        {
            if (isOpen) buttonUseable?.Invoke(true); 
        });
    }

    public void Close()
    {
        isOpen = false;
        Time.timeScale = 1;

        buttonUseable?.Invoke(false);
        CanvasFade.Instance.FadeCanvas(canvas, false, 0.5f, () =>
        {
            if (isOpen) buttonUseable?.Invoke(false);
        });
    }
}
