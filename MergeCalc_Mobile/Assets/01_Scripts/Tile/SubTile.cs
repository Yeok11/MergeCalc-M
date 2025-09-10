public class SubTile : MoveTile
{
    public void RemoveTile(MainTile _mTile)
    {
        moveFin += () =>
        {
            _mTile.MergeUpdate();
        };
    }
}
