using TMPro;
using UnityEngine;

public class ExplainPanel : Panel
{
    [SerializeField] private TextMeshProUGUI nameTmp, explainTmp;

    public void Open(ModeSO _mode)
    {
        nameTmp.SetText(_mode.mode + " Mode");
        explainTmp.SetText(_mode.explain);

        base.Open();
    }
}
