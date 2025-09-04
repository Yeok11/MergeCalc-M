public class SubTile : MoveTile
{
    public void RemoveTile(MainTile _mTile)
    {
        moveFin += () =>
        {
            TilePooling.Instance.Push(this);
            _mTile.TextUpdate();
        };
    }
}
