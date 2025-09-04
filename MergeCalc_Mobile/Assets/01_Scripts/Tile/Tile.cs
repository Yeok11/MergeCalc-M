using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileData data;
    protected TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    public virtual void Init(TileData _tileData)
    {
        data = _tileData;
        TextUpdate();
    }

    public virtual void TextUpdate()
    {
        string str = "";
        switch (data.calc)
        {
            case CalcEnum.Plus: str = "+"; break;
            case CalcEnum.Multiple: str = "x"; break;
            case CalcEnum.Divide: str = "/"; break;
        }
        tmp.SetText(str + data.num);
    }
}
