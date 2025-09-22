public class SubTile : MoveTile
{
    public void RemoveTile(MainTile _mTile)
    {
        moveFin.AddListener(() => _mTile.MergeUpdate());
    }
}