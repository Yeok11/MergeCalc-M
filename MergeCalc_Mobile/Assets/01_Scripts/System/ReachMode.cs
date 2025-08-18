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
            LeaveTime += value;
            if (LeaveTime > gameTime) LeaveTime = gameTime;
        }
    }
    private float LeaveTime, gameTime = 60;

    protected override void Start()
    {
        base.Start();

        dragEvent += () => Debug.Log("Drag Finish");
        dragEvent += NextTileCheck;
        dragEvent += DeadCheck;

        mainTile.mergeEvent += TargetCheck;

        SetTarget();
        SetNextTiles();

        leaveTime = gameTime;
        timer.fillAmount = 1;
    }

    private void NextTileCheck()
    {
        if (showTiles.Count > nextTiles.Count) SetNextTiles();
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

        for (int calc = 0; calc < 4; calc++)
        {
            for (int num = 1; num <= 5; num++)
            {
                if (calc > 1)
                {
                    if (num == 1) continue;
                    if (num > 3) break;
                }

                _tileDatas.Add(new((CalcEnum)calc, num, tileMoveTime));
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

    private void SetTarget()
    {
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

        targetRange = 20 + (score / 10 * 5);
        SetTarget();
    }
}
