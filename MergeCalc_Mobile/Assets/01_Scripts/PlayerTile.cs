using UnityEngine;

public class PlayerTile : Tile
{
    public override void Init(int _v)
    {
        calc = Calculate.Plus;
        num = _v;

        TextUpdate();
    }

    protected override void TextUpdate()
    {
        if (num < 0) tmp.text = "-" + num.ToString();
        else tmp.text = num.ToString();
    }
}
