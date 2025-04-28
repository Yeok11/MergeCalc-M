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

    public void Init(Tile _main)
    {
        Transform trm = backTiles[12];
        canUseTiles.Remove(trm);
        gameTiles[2, 2] = _main;

        _main.transform.position = trm.position;
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

        canUseTiles.Add(gameTiles[_vec.y, _vec.x].transform);
        canUseTiles.Remove(gameTiles[curY, curX].transform);

        return new Vector2Int(curX, curY);
    }

    public void OnDrag(Dir _dir)
    {
        int n = 0, v = 1, m = 5;

        if (_dir == Dir.Down || _dir == Dir.Right) { n = 4; v = m = -1; }

        for (int loop = 0; loop < 5; loop++)
        {
            for (int i = n; i != m; i+=v)
            {
                if (_dir == Dir.Up || _dir == Dir.Down)
                {
                    if (gameTiles[i, loop] == null) continue;

                    Vector2Int v2Int = GetDestination(new Vector2Int(loop, i), _dir);

                    if (v2Int.y == i) return;

                    gameTiles[i, loop].Move(backTiles[v2Int.y * 5 + v2Int.x].transform);
                    gameTiles[v2Int.y, v2Int.x] = gameTiles[i, loop];
                    gameTiles[i, loop] = null;
                }
                else
                {
                    if (gameTiles[loop, i] == null) continue;

                    Vector2Int v2Int = GetDestination(new Vector2Int(i, loop), _dir);

                    if (v2Int.x == i) return;

                    gameTiles[loop, i].Move(backTiles[v2Int.y * 5 + v2Int.x].transform);
                    gameTiles[v2Int.y, v2Int.x] = gameTiles[loop, i];
                    gameTiles[loop, i] = null;

                    //if (gameTiles[loop, i] == null) continue;
                    //MoveTile(new Vector2Int(n, loop), _dir);
                    ////gameTiles[loop, n];
                }
            }
        }

        Debug.Log("All Fin -> Dir : " + _dir);
    }
    #endregion
}
