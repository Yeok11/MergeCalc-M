using UnityEngine.Events;

public class SubTile : MoveTile
{
    public override void Init(TileData _tileData, UnityAction _moveFin)
    {
        base.Init(_tileData, _moveFin);
    }

    public void RemoveTile(MainTile _mTile)
    {
        moveFin += () =>
        {
            TilePooling.Instance.Push(this);
            _mTile.TextUpdate();
        };
    }
}
