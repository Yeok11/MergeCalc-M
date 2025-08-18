using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReachMode : GameSystem
{
    [SerializeField] private TextMeshProUGUI targetText;
    private int target, range = 25;

    protected override void Start()
    {
        base.Start();

        dragEvent += () => Debug.Log("Drag Finish");
        dragEvent += NextTileCheck;
        dragEvent += DeadCheck;

        mainTile.mergeEvent += TargetCheck;

        SetTarget();

        SetNextTiles();
    }

    private void NextTileCheck()
    {
        if (showTiles.Count > nextTiles.Count) SetNextTiles();
    }

    private void DeadCheck()
    {
        int n = mainTile.GetValue();

        if (n > range || n < -range)
        {
            Debug.Log("is Dead");
            GameData.canDrag = false;

            GameData.DataUpdate(score, Mode.Reach);

            UnityEngine.SceneManagement.SceneManager.LoadScene("Game Over");
        }
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
            target = Random.Range(-20, 21);
        }
        while (target == mainTile.GetValue());

        targetText.SetText($"Limit : {range} / Goal : {target}");
    }

    private void TargetCheck(int _value)
    {
        if (target != _value) return;

        AddScore(1);
        target = Random.Range(1, 21);

        SetTarget();
    }
}
