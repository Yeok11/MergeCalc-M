using UnityEngine;

public class ShowTile : Tile
{
    private bool isNull = true;

    public bool DataEmpty() => isNull;

    public override void Init(TileData _tileData)
    {
        base.Init(_tileData);

        isNull = false;
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }
    
    public Tile Use()
    {
        isNull = true;
        gameObject.SetActive(false);
        return this;
    }
}
