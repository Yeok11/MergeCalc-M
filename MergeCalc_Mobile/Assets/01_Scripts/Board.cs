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

    private void Awake()
    {
        for (int i = 0; i < groupPos.childCount; i++)
        {
            backTiles[i] = groupPos.GetChild(i);
            canUseTiles.Add(backTiles[i]);
        }
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

    private Vector2Int GetDestination(Vector2Int _pos, Dir _dir)
    {
        int curX = _pos.x, curY = _pos.y;

        switch (_dir)
        {
            case Dir.Up:    
                for (int y = _pos.y - 1; y >= 0; --y)   
                {
                    if (CheckDestination(_pos, new(_pos.x, y)) == false) break;
                    curY = y;
                }
                break;

            case Dir.Down:  
                for (int y = _pos.y + 1; y < 5; ++y)
                { 
                    if (CheckDestination(_pos, new(_pos.x, y)) == false) break;
                    curY = y; 
                } 
                break;

            case Dir.Left:  
                for (int x = _pos.x - 1; x >= 0; --x)   
                {
                    if (CheckDestination(_pos, new(x, _pos.y)) == false) break;
                    curX = x; 
                } 
                break;
            
            case Dir.Right: 
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

    public void OnDragEvent(Dir _dir)
    {
        usedMerge = false;

        int iStart = 0, iStep = 1, iEnd = 5;
        if (_dir == Dir.Down || _dir == Dir.Right) 
        { 
            iStart = 4; 
            iStep = iEnd = -1; 
        }
        
        List<MoveTile> _moveTiles = new();
        bool verticalCheck = (_dir == Dir.Up || _dir == Dir.Down);

        for (int loop = 0; loop < 5; loop++)
        {
            for (int i = iStart; i != iEnd; i += iStep)
            {
                int x = verticalCheck ? loop : i;
                int y = verticalCheck ? i : loop;

                var tile = gameBoard[y, x];
                if (tile == null) continue;

                var dest = GetDestination(new(x, y), _dir);

                if (verticalCheck ? dest.y == i : dest.x == i) continue;

                if (usedMerge)
                {
                    if (tile == mainTile)
                    {
                        Debug.Log("Player Tile Merge with " + dest);
                        mainTile.Merge(gameBoard[dest.y, dest.x], true);
                    }

                    if (gameBoard[dest.y, dest.x] == mainTile)
                    {
                        Debug.Log("Player Tile Merge with " + new Vector2Int(y, x));
                        mainTile.Merge(tile, false);
                        (tile as SubTile).RemoveTile(mainTile);
                    }
                }

                tile.MoveSet(backTiles[dest.y * 5 + dest.x].transform);
                _moveTiles.Add(tile);

                if (gameBoard[dest.y, dest.x] != mainTile) gameBoard[dest.y, dest.x] = tile;
                gameBoard[y, x] = null;
            }
        }

        mainTile.transform.SetAsLastSibling();
        GameSystem.Instance.Move(_moveTiles);
    }
    #endregion
}
