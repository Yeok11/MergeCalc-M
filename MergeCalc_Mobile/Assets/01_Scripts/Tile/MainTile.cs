using UnityEngine.Events;

public class MainTile : MoveTile
{
    internal MoveTile mergeTile;
    internal UnityAction<int> mergeEvent;

    public int GetValue() => data.num;

    public override void Init(TileData _tileData, UnityAction _moveFin)
    {
        base.Init(_tileData, _moveFin);

        moveFin += TextUpdate;
        moveFin += MergeUpdate;
    }

    public void AddMoveEvent(UnityAction _action)
    {
        moveFin += _action;
    }

    public override void TextUpdate()
    {
        tmp.text = data.num.ToString();
    }

    public void Merge(MoveTile _tile, bool _newMergeTile)
    {
        UnityEngine.Debug.Log("Data Merge");

        data.num = Calculator.Calc(_tile.data, data.num);

        mergeEvent?.Invoke(data.num);

        if (_newMergeTile) mergeTile = _tile;
    }

    private void MergeUpdate()
    {
        if (mergeTile != null)
        {
            TilePooling.Instance.Push(mergeTile);
            mergeTile = null;
        }
    }
}