using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LiveMode : GameSystem
{
    [SerializeField] private TextMeshProUGUI leftTmp, rangeTmp;
    private int nextTileCnt, range;

    protected override void Start()
    {
        base.Start();

        dragEvent.AddListener(DeadCheck);

        SetNextTiles();
        GameData.canDrag = true;
    }

    private void DeadCheck()
    {
        int n = mainTile.GetValue();
        if (n > range || n < -range) GameOver();
    }

    protected override void GameOver()
    {
        GameData.canDrag = false;
        GameData.ScoreUpdate(score, Mode.Live);
        base.GameOver();
    }

    private void StageClear()
    {
        AddScore();
        SetNextTiles();
    }

    protected override void SetNextTiles()
    {
        // (Min / Max)
        Vector2Int _PM = new((score / 3) + 1, ((score + 1) / 2) + 5), _MD = new(score / 4 + 2, (score + 1) / 2 + 2);
        range = ((score / 2) + 1) * 10;
        showTileNum = 0;

        rangeTmp.SetText($"Max : {range} / Min : -{range}");

        List<TileData> _tileDatas = new();

        //Plus, Minus
        for (int num = _PM.x; num <= _PM.y; num++)
        {
            _tileDatas.Add(new(CalcEnum.Plus, num, tileMoveTime, boundValue));
            _tileDatas.Add(new(CalcEnum.Minus, num, tileMoveTime, boundValue));
        }

        //Multiple, Divide
        for (int num = _MD.x; num <= _MD.y; num++)
        {
            _tileDatas.Add(new(CalcEnum.Multiple, num, tileMoveTime, boundValue));
            _tileDatas.Add(new(CalcEnum.Divide, num, tileMoveTime, boundValue));
        }

        while (_tileDatas.Count != 0)
        {
            int _rand = Random.Range(0, _tileDatas.Count);
            nextTiles.Enqueue(_tileDatas[_rand]);
            _tileDatas.RemoveAt(_rand);
        }

        nextTileCnt = nextTiles.Count;

        LeftTileCheck();
        UpdateUiTiles();
    }

    protected override void Spawn()
    {
        base.Spawn();
        nextTileCnt--;
        LeftTileCheck();
    }

    private void LeftTileCheck()
    {
        leftTmp.SetText($"Left Tile ({nextTileCnt})");

        if (nextTileCnt == 0)
        {
            GameData.canDrag = false;
            Invoke("StageClear", 1);
        }
        else
            GameData.canDrag = true;
    }

    protected override void ApplicationQuit()
    {
        GameData.ScoreUpdate(score, Mode.Live);
    }
}
