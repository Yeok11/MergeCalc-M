using UnityEngine.Events;

public class MainTile : MoveTile
{
    private MoveTile mergeTile;
    public event UnityAction<int> mergeEvent;

    public int GetValue() => data.num;

    public override void Init(TileData _tileData, UnityAction _moveFin)
    {
        base.Init(_tileData, _moveFin);

        moveFin += TextUpdate;
        moveFin += MergeUpdate;

        mergeEvent += (int _n) => TileAnime();
    }

    public override void TextUpdate() => tmp.text = data.num.ToString();

    public void Merge(MoveTile _tile)
    {
        data.num = Calculator.Calc(_tile.data, data.num);

        mergeEvent?.Invoke(data.num);
        mergeTile ??= _tile;
    }

    private void MergeUpdate()
    {
        if (mergeTile != null)
        {
            TilePooling.Instance.Push(mergeTile);
            mergeTile = null;
        }
    }

    public override void Move()
    {
        isMerging = mergeTile;
        base.Move();
        isMerging = false;
    }
}