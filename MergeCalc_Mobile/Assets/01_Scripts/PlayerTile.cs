using UnityEngine;
using UnityEngine.Events;

public class PlayerTile : Tile
{
    public override void Init(int _v, UnityAction _moveFin)
    {
        calc = Calculate.Plus;
        num = _v;
        moveFin = _moveFin;

        TextUpdate();
    }

    protected override void TextUpdate()
    {
        if (num < 0) tmp.text = "-" + num.ToString();
        else tmp.text = num.ToString();
    }
}
