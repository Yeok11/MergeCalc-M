using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReachMode : GameSystem
{
    [SerializeField] private TextMeshProUGUI targetText;
    private int target, targetRange = 20;

    [SerializeField] private Image timer;
    private float leaveTime
    {
        get => LeaveTime;
        set
        {
            LeaveTime = value;
            if (LeaveTime > gameTime) LeaveTime = gameTime;
        }
    }
    private float LeaveTime, gameTime = 60;

    protected override void Start()
    {
        initValue = 0;
        base.Start();

        dragEvent += NextTileCheck;
        dragEvent += DeadCheck;

        mainTile.mergeEvent += TargetCheck;

        SetTarget();

        leaveTime = gameTime;
        timer.fillAmount = 1;

        SetNextTiles();
        GameData.canDrag = true;
    }

    private void NextTileCheck()
    {
        if (showTiles.Count > nextTiles.Count) SetNextTiles();
        GameData.canDrag = true;
    }

    private void DeadCheck()
    {
        if (0 >= leaveTime)
        {
            Debug.Log("is Dead");
            GameData.canDrag = false;

            GameData.DataUpdate(score, Mode.Reach);

            UnityEngine.SceneManagement.SceneManager.LoadScene("Game Over");
        }
    }

    private void Update()
    {
        leaveTime -= Time.deltaTime;
        timer.fillAmount = leaveTime / gameTime;
    }

    public override void SetNextTiles()
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
        targetRange = 5 + (score / 10 * 3);

        do
        {
            target = Random.Range(-targetRange, targetRange+1);
        }
        while (target == mainTile.GetValue());

        targetText.SetText($"Target : {target}");
    }

    private void TargetCheck(int _value)
    {
        if (target != _value) return;

        leaveTime += 10;
        AddScore(1);
        SetTarget();
    }
}
