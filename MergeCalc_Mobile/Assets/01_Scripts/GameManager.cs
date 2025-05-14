using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private PlayerTile mainTile;
    [SerializeField] private Transform tilePos;
    private int limitCnt = 5, moveTileCnt, checkTileCnt;

    private Board board;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        board = GetComponent<Board>();
    }

    private void Start()
    {
        PlayerTile _mainTile = Instantiate(mainTile, tilePos);
        _mainTile.Init(1, ()=>MoveCheck());
        board.Init(_mainTile);
    }

    public void Spawn()
    {
        Tile tile = TilePooling.Instance.Pop(tilePos);
        tile.Init(limitCnt, ()=>MoveCheck());

        board.Spawn(tile);
    }

    private void MoveCheck()
    {
        if (checkTileCnt++ != moveTileCnt) return;

        Spawn();
    }

    public void Move(List<Tile> _moveTiles)
    {
        moveTileCnt = _moveTiles.Count;
        checkTileCnt = 1;

        foreach (Tile item in _moveTiles) item.Move();
    }
}
