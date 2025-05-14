using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform groupPos;
    
    private Transform[] backTiles = new Transform[25];
    private List<Transform> canUseTiles = new List<Transform>();
    private Tile[,] gameTiles = new Tile[5,5];




    private void Awake()
    {
        for (int i = 0; i < groupPos.childCount; i++)
        {
            backTiles[i] = groupPos.GetChild(i);
            canUseTiles.Add(backTiles[i]);
        }
    }

    public void Init(PlayerTile _tile)
    {
        Transform trm = backTiles[12];
        canUseTiles.Remove(trm);
        gameTiles[2, 2] = _tile;

        _tile.transform.position = trm.position;
    }

    public void Spawn(Tile _tile)
    {
        Transform trm = canUseTiles[Random.Range(0, canUseTiles.Count)];
        canUseTiles.Remove(trm);

        _tile.transform.position = trm.position;
        int v = trm.GetSiblingIndex();
        gameTiles[v / 5, v % 5] = _tile;
    }

    #region Drag
    private Vector2Int GetDestination(Vector2Int _vec, Dir _dir)
    {
        int curX = _vec.x, curY = _vec.y;

        switch (_dir)
        {
            case Dir.Up:    for (int y = _vec.y - 1; y >= 0; --y)   { if (gameTiles[y, _vec.x] != null) break; curY = y; } break;
            case Dir.Down:  for (int y = _vec.y + 1; y < 5; ++y)    { if (gameTiles[y, _vec.x] != null) break; curY = y; } break;
            case Dir.Left:  for (int x = _vec.x - 1; x >= 0; --x)   { if (gameTiles[_vec.y, x] != null) break; curX = x; } break;
            case Dir.Right: for (int x = _vec.x + 1; x < 5; ++x)    { if (gameTiles[_vec.y, x] != null) break; curX = x; } break;
        }

        canUseTiles.Add(backTiles[_vec.y * 5 + _vec.x]);
        canUseTiles.Remove(backTiles[curY * 5 + curX]);

        return new Vector2Int(curX, curY);
    }

    public void OnDragEvent(Dir _dir)
    {
        int x, y, n = 0, v = 1, m = 5;

        Debug.Log(_dir);
        if (_dir == Dir.Down || _dir == Dir.Right) { n = 4; v = m = -1; }
        bool verticalCheck = (_dir == Dir.Up || _dir == Dir.Down);

        for (int loop = 0; loop < 5; loop++)
        {
            for (int i = n; i != m; i+=v)
            {
                if (verticalCheck) { x = loop; y = i; }
                else { x = i; y = loop; }

                if (gameTiles[y, x] == null) continue;
                Vector2Int v2Int = GetDestination(new Vector2Int(x, y), _dir);

                if (verticalCheck && v2Int.y == i) continue;
                else if (!verticalCheck && v2Int.x == i) continue;

                gameTiles[y, x].Move(backTiles[v2Int.y * 5 + v2Int.x].transform);
                gameTiles[v2Int.y, v2Int.x] = gameTiles[y, x];
                gameTiles[y, x] = null;
            }
        }
    }
    #endregion
}
