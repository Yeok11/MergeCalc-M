using TMPro;
using UnityEngine;

public class ExplainPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI nameTmp, explainTmp;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.gameObject.SetActive(false);
    }

    public void Open(ModeSO _mode)
    {
        nameTmp.SetText(_mode.mode + " Mode");
        explainTmp.SetText(_mode.explain);

        canvasGroup.gameObject.SetActive(true);
        CanvasFade.Instance.FadeCanvas(canvasGroup, true, 0.15f, null);
    }

    public void Close()
    {
        CanvasFade.Instance.FadeCanvas(canvasGroup, false, 0.15f, () => canvasGroup.gameObject.SetActive(false));
    }
}
