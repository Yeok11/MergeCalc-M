using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReachMode : GameSystem
{
    [SerializeField] private TextMeshProUGUI targetText;
    private int target;

    [SerializeField] private Image timer;
    private float leaveTime
    {
        get => LeaveTime;
        set
        {
            LeaveTime = value;
            if (LeaveTime > gameTime) LeaveTime = gameTime;
            if (LeaveTime <= 0) GameOver();
        }
    }
    private float LeaveTime, gameTime = 60;

    private bool timeStream;

    protected override void Start()
    {
        initValue = 0;
        base.Start();

        dragEvent.AddListener(OnDrageEvent);

        SetTarget();

        timeStream = false;

        leaveTime = gameTime;
        timer.fillAmount = 1;

        SetNextTiles();
        GameData.canDrag = true;
    }

    private void Update()
    {
        if (!timeStream) return;

        leaveTime -= Time.deltaTime;
        timer.fillAmount = leaveTime / gameTime;
    }

    #region Event Subscribe
    protected override void OnMergeEvent(int _n)
    {
        base.OnMergeEvent(_n);
        TargetCheck(_n);
    }

    private void OnDrageEvent()
    {
        NextTileCheck();
        if(!timeStream) timeStream = true;
    }
    #endregion

    protected override void GameOver()
    {
        GameData.canDrag = false;
        GameData.ScoreUpdate(score, Mode.Reach);
        base.GameOver();
    }

    private void NextTileCheck()
    {
        if (showTiles.Count > nextTiles.Count) SetNextTiles();
        GameData.canDrag = true;
    }

    protected override void SetNextTiles()
    {
        List<TileData> _tileDatas = new();

        int pMax = 3 + (score / 10 * 3), mMax = 1 + (score + 5) / 10;

        for (int num = 1; num <= pMax; num++)
        {
            _tileDatas.Add(new(CalcEnum.Plus, num, tileMoveTime, boundValue));
            _tileDatas.Add(new(CalcEnum.Minus, num, tileMoveTime, boundValue));
        }

        for (int num = 2; num <= mMax; num++)
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

        UpdateUiTiles();
    }

    private void SetTarget()
    {
        int _targetRange = 5 + (score / 10 * 3);

        do
        {
            target = Random.Range(-_targetRange, _targetRange + 1);
        }
        while (target == mainTile.GetValue());

        targetText.SetText($"Target : {target}");
    }

    private void TargetCheck(int _value)
    {
        if (target != _value) return;

        leaveTime += 10;
        AddScore();
        SetTarget();
    }

    protected override void ApplicationQuit()
    {
        GameData.ScoreUpdate(score, Mode.Reach);
    }
}
