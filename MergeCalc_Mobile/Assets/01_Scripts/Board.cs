using UnityEngine;

public class Board : MonoBehaviour
{
    private Transform groupPos;
    private Transform[] backTiles = new Transform[25];

    private Tile[,] gameTiles = new Tile[5,5];


    private void Awake()
    {
        for (int i = 0; i < groupPos.childCount; i++) backTiles[i] = groupPos.GetChild(i);
    }

    private Transform MoveTile(Vector2Int _vec, Dir _dir)
    {
        int curX = _vec.x, curY = _vec.y;

        switch (_dir)
        {
            case Dir.Up:    for (int y = _vec.y; y < 5; ++y) if (gameTiles[y, _vec.x] != null) { curY = y - 1; break; } break;
            case Dir.Down:  for (int y = _vec.y; y >= 0; --y) if (gameTiles[y, _vec.x] != null) { curY = y + 1; break; } break;
            case Dir.Right: for (int x = _vec.x; x < 5; ++x) if (gameTiles[_vec.y, x] != null) { curX = x - 1; break; } break;
            case Dir.Left:  for (int x = _vec.x; x >= 0; --x) if (gameTiles[_vec.y, x] != null) { curX = x + 1; break; } break;
        }

        return gameTiles[curY, curX].transform;
    }

    public void OnDrag(Dir _dir)
    {
        int n = 0, v = 1, m = 5;

        if (_dir == Dir.Down || _dir == Dir.Right)
        {
            n = 4;
            v = -1;
            m = -1;
        }

        for (int loop = 0; loop < 5; loop++)
        {
            while (n != m)
            {
                if(_dir == Dir.Up || _dir == Dir.Down)
                {
                    if (gameTiles[n, loop] == null) continue;
                    gameTiles[n, loop].Move(MoveTile(new Vector2Int(loop, n), _dir));
                }
                else
                {
                    if (gameTiles[loop, n] == null) continue;
                    //MoveTile(new Vector2Int(n, loop), _dir);
                    ////gameTiles[loop, n];

                }

                n += v;
            }
        }
    }
}
