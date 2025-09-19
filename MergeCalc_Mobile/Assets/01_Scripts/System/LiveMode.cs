using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LiveMode : GameSystem
{
    [SerializeField] private TextMeshProUGUI leftText, rangeText;
    private int cnt, range;

    protected override void Start()
    {
        base.Start();

        dragEvent += DeadCheck;

        SetNextTiles();
        GameData.canDrag = true;
    }

    private void DeadCheck()
    {
        int n = mainTile.GetValue();

        if (n > range || n < -range) GameOver();
    }

    public override void GameOver()
    {
        GameData.canDrag = false;
        GameData.ScoreUpdate(score, Mode.Live);
        base.GameOver();
    }

    private void StageClear()
    {
        AddScore(1);
        SetNextTiles();
    }

    public override void SetNextTiles()
    {
        // (Min / Max)
        Vector2Int _PM = new((score / 3) + 1, ((score + 1) / 2) + 5), _MD = new(score / 4 + 2, (score + 1) / 2 + 2);
        range = ((score / 2) + 1) * 10;
        showTileNum = 0;

        rangeText.SetText($"Max : {range} / Min : -{range}");

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

        cnt = nextTiles.Count;

        LeftTileCheck();
        UpdateUiTiles();
    }

    protected override void Spawn()
    {
        base.Spawn();
        cnt--;
        LeftTileCheck();
    }

    private void LeftTileCheck()
    {
        leftText.SetText($"Left Tile ({cnt})");

        if (cnt == 0)
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
