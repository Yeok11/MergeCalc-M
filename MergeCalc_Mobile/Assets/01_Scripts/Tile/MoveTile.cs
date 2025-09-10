using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class MoveTile : Tile
{
    protected UnityAction moveFin;
    protected Transform destination;
    protected Direction direction;

    private float time, bound, animeValue = 0.8f;
    protected bool isMerging;

    public virtual void Init(Tile _tile, UnityAction _ua) => Init(_tile.data, _ua);

    public virtual void Init(TileData _tileData, UnityAction _moveFin)
    {
        base.Init(_tileData);

        time = _tileData.time;
        moveFin = _moveFin;
        bound = _tileData.bound;
        isMerging = false;

        transform.localScale = new(0.8f, 0.8f, 0.8f);
        gameObject.SetActive(true);

        transform.DOScale(Vector3.one, 0.2f);
    }

    public void MoveSet(Transform _trm, Direction _direction, bool _merge)
    {
        destination = _trm;
        direction = _direction;
        isMerging = _merge;
    }

    protected void TileAnime()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(animeValue, 0.1f));
        seq.Append(transform.DOScale(1, 0.2f));
    }

    public virtual void Move()
    {
        Vector3 _addPos = new(0, bound);
        switch (direction)
        {
            case Direction.Down:  _addPos = new(0, -bound);
                break;

            case Direction.Left:  _addPos = new(-bound, 0);
                break;

            case Direction.Right: _addPos = new(bound, 0);
                break;
        }

        float _animeTime = time;
        Sequence seq = DOTween.Sequence();

        if (isMerging)
        {
            transform.localScale = new(animeValue, animeValue);
        }
        else
        {
            seq.Append(transform.DOMove(destination.position + _addPos, _animeTime * 0.7f));
            _animeTime *= 0.3f;
        }

        seq.Append(transform.DOMove(destination.position, _animeTime));
        seq.AppendCallback(() => moveFin?.Invoke());
    }
}
