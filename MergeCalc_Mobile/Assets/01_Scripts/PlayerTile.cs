using UnityEngine;

public class PlayerTile : Tile
{
    public override void Init(int _v)
    {
        tmp.text = _v.ToString();
    }
}
