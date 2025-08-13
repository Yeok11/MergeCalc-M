using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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

        SetTarget(false);

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
            canDrag = false;

            GameData.DataUpdate(score);

            UnityEngine.SceneManagement.SceneManager.LoadScene("Game Over");
        }
    }

    public override void SetNextTiles()
    {
        List<TileData> _tileDatas = new();

        for (int calc = 0; calc < 4; calc++)
        {
            for (int num = 1; num <= 7; num++)
            {
                if (calc > 1)
                {
                    if (num == 1) continue;
                    if (num > 5) break;
                }

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

    private void SetTarget(bool _includeZero)
    {
        if (_includeZero)
            target = Random.Range(-20, 21);
        else
            target = Random.Range(1,21) * (Random.Range(0, 2) == 0 ? -1 : 1);

        targetText.SetText($"Limit : {range} / Goal : {target}");
    }

    private void TargetCheck(int _value)
    {
        if (target != _value) return;

        AddScore(1);
        target = Random.Range(1, 21);

        SetTarget(true);
    }
}
