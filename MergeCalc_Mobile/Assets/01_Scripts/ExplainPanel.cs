using TMPro;
using UnityEngine;

public class ExplainPanel : Panel
{
    [SerializeField] private TextMeshProUGUI nameTmp, explainTmp;
    [SerializeField] private Home home;

    public override void Init()
    {
        if (home == null) home = FindAnyObjectByType<Home>();

        onOpenEvent += () => DataUpdate(home.GetMode());
        base.Init();
    }

    public void DataUpdate(ModeSO _mode)
    {
        nameTmp.SetText(_mode.mode + " Mode");
        explainTmp.SetText(_mode.explain);
    }
}
