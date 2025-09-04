using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform groupPos;
    
    private Transform[] backTiles = new Transform[25];
    private List<Transform> canUseTiles = new List<Transform>();
    private MoveTile[,] gameBoard = new MoveTile[5,5];

    private MainTile mainTile;
    private bool usedMerge;

    [SerializeField] private DragSystem dragSystem;

    private void Awake()
    {
        for (int i = 0; i < groupPos.childCount; i++)
        {
            backTiles[i] = groupPos.GetChild(i);
            canUseTiles.Add(backTiles[i]);
        }
    }

    private void Start()
    {
        dragSystem.dragEvent += OnDragEvent;
    }

    public void Init(MainTile _tile)
    {
        Transform trm = backTiles[12];
        canUseTiles.Remove(trm);

        mainTile = _tile;
        gameBoard[2, 2] = _tile;

        _tile.transform.localPosition = Vector3.zero;
    }

    public void Spawn(MoveTile _tile)
    {
        Transform trm = canUseTiles[Random.Range(0, canUseTiles.Count)];
        canUseTiles.Remove(trm);

        _tile.transform.position = trm.position;

        int v = trm.GetSiblingIndex();
        gameBoard[v / 5, v % 5] = _tile;
    }

    #region Drag
    private bool CheckDestination(Vector2Int _startPos, Vector2Int _checkPos)
    {
        if (gameBoard[_checkPos.y, _checkPos.x] == null) return true;

        if (usedMerge) return false;

        if (gameBoard[_startPos.y, _startPos.x] is MainTile || gameBoard[_checkPos.y, _checkPos.x] is MainTile)
        {
            usedMerge = true;
            return true;
        }

        return false;
    }

    private Vector2Int GetDestination(Vector2Int _pos, Direction _dir)
    {
        int curX = _pos.x, curY = _pos.y;

        switch (_dir)
        {
            case Direction.Up:    
                for (int y = _pos.y - 1; y >= 0; --y)   
                {
                    if (CheckDestination(_pos, new(_pos.x, y)) == false) break;
                    curY = y;
                }
                break;

            case Direction.Down:  
                for (int y = _pos.y + 1; y < 5; ++y)
                { 
                    if (CheckDestination(_pos, new(_pos.x, y)) == false) break;
                    curY = y; 
                } 
                break;

            case Direction.Left:  
                for (int x = _pos.x - 1; x >= 0; --x)   
                {
                    if (CheckDestination(_pos, new(x, _pos.y)) == false) break;
                    curX = x; 
                } 
                break;
            
            case Direction.Right: 
                for (int x = _pos.x + 1; x < 5; ++x)
                { 
                    if (CheckDestination(_pos, new(x, _pos.y)) == false) break;
                    curX = x; 
                } 
                break;
        }

        canUseTiles.Add(backTiles[_pos.y * 5 + _pos.x]);
        canUseTiles.Remove(backTiles[curY * 5 + curX]);

        return new Vector2Int(curX, curY);
    }

    public void OnDragEvent(Direction _direction)
    {
        usedMerge = false;

        int _iStart = 0, _iStep = 1, _iEnd = 5;
        if (_direction == Direction.Down || _direction == Direction.Right) 
        { 
            _iStart = 4; 
            _iStep = _iEnd = -1; 
        }
        
        List<MoveTile> _moveTiles = new();
        bool _isVertical = (_direction == Direction.Up || _direction == Direction.Down);

        for (int loop = 0; loop < 5; loop++)
        {
            for (int i = _iStart; i != _iEnd; i += _iStep)
            {
                int x = _isVertical ? loop : i, y = _isVertical ? i : loop;
                var tile = gameBoard[y, x];

                if (tile == null) continue;

                var _dest = GetDestination(new(x, y), _direction);

                if (_isVertical ? _dest.y == i : _dest.x == i) continue;

                bool _mergedTile = false;

                if (usedMerge)
                {
                    if (tile == mainTile)
                        mainTile.Merge(gameBoard[_dest.y, _dest.x]);

                    if (gameBoard[_dest.y, _dest.x] == mainTile)
                    {
                        _mergedTile = true;
                        mainTile.Merge(tile);
                        (tile as SubTile).RemoveTile(mainTile);
                    }
                }

                tile.MoveSet(backTiles[_dest.y * 5 + _dest.x].transform, _direction, _mergedTile);
                _moveTiles.Add(tile);

                if (gameBoard[_dest.y, _dest.x] != mainTile) gameBoard[_dest.y, _dest.x] = tile;
                gameBoard[y, x] = null;
            }
        }

        mainTile.transform.SetAsLastSibling();
        GameSystem.Instance.Move(_moveTiles);
    }
    #endregion
}
