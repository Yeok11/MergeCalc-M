using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;

    protected MainTile mainTile;
    [SerializeField] private MainTile mainTilePrefab;
    [SerializeField] private Transform tilePos;
    [SerializeField] protected TextMeshProUGUI scoreTmp;
    protected int limitCnt = 5, moveTileCnt, checkTileCnt, score;

    protected Queue<TileData> nextTiles = new();

    [SerializeField] protected List<ShowTile> showTiles;
    protected int showTileNum = 0;

    private Board board;


    public bool canDrag = false;
    protected UnityAction dragEvent;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        board = GetComponent<Board>();
    }

    protected virtual void Start()
    {
        mainTile = Instantiate(mainTilePrefab, tilePos);
        mainTile.Init(new TileData(CalcEnum.Plus, 1), MoveFinCheck);
        
        score = 0;
        scoreTmp.SetText("0");
        
        board.Init(mainTile);
    }

    protected virtual void AddScore(int _value)
    {
        score += _value;
        scoreTmp.SetText(score.ToString());
    }

    public virtual void SetNextTiles()
    {
        List<TileData> _tileDatas = new();

        for (int calc = 0; calc < 4; calc++)
        {
            for (int num = 1; num <= limitCnt; num++)
            {
                if (calc > 1 && num == 1) continue;

                _tileDatas.Add(new((CalcEnum)calc, num));
            }
        }

        while (_tileDatas.Count != 0)
        {
            int _rand = Random.Range(0, _tileDatas.Count);
            nextTiles.Enqueue(_tileDatas[_rand]);
            _tileDatas.RemoveAt(_rand);
        }

        UpdateUiTiles();
    }

    protected void UpdateUiTiles()
    {
        foreach (var tile in showTiles)
        {
            if(tile.DataEmpty())
            {
                if (nextTiles.Count == 0) return;

                tile.Init(nextTiles.Dequeue());
            }
        }

        canDrag = true;
    }

    protected virtual void Spawn()
    {
        MoveTile tile = TilePooling.Instance.Pop(tilePos);
        tile.Init(showTiles[showTileNum].Use(), MoveFinCheck);

        board.Spawn(tile);

        showTileNum += 1;
        if (showTileNum == showTiles.Count) showTileNum = 0;

        UpdateUiTiles();
    }

    private void MoveFinCheck()
    {
        if (checkTileCnt++ == moveTileCnt)
        {
            Spawn();
            UpdateUiTiles();
            dragEvent?.Invoke();
        }
    }

    public void Move(List<MoveTile> _moveTiles)
    {
        moveTileCnt = _moveTiles.Count;
        checkTileCnt = 1;

        foreach (MoveTile item in _moveTiles) item.Move();
    }
}
