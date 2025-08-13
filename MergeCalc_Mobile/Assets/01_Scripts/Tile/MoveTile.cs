using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class MoveTile : Tile
{
    [SerializeField] private float t = 0.4f;

    protected UnityAction moveFin;
    protected Transform destination;
    
    public virtual void Init(Tile _tile, UnityAction _ua) => Init(_tile.data, _ua);

    public virtual void Init(TileData _tileData, UnityAction _moveFin)
    {
        base.Init(_tileData);

        moveFin = _moveFin;
        gameObject.SetActive(true);
    }

    public void MoveSet(Transform _trm)
    {
        destination = _trm;
    }

    public void Move()
    {
        transform.DOMove(destination.position, t).OnComplete(() => moveFin?.Invoke());
    }
}
