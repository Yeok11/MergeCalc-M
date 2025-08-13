using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LiveMode : GameSystem
{
    [SerializeField] private TextMeshProUGUI leftText, rangeText;
    private int cnt, stageNum = 0, range;

    private UnityAction stageClearEvent;

    protected override void Start()
    {
        base.Start();

        dragEvent += () => Debug.Log("Drag Finish");
        dragEvent += DeadCheck;

        mainTile.mergeEvent += (int _i) => AddScore(1);

        SetNextStage();

        stageClearEvent += () => AddScore(5);
    }

    public void DeadCheck()
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

    private void SetNextStage()
    {
        Debug.Log("Set Next Stage");

        stageClearEvent?.Invoke();

        limitCnt = 5 + (stageNum++ / 2);
        range = (limitCnt - 1) * 5 + (stageNum % 2) * 5;
        showTileNum = 0;

        rangeText.SetText($"Max : {range} / Min : -{range}");

        SetNextTiles();
    }

    public override void SetNextTiles()
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

        cnt = nextTiles.Count;

        LeftMessageUpdate();
        UpdateUiTiles();
    }

    protected override void Spawn()
    {
        base.Spawn();

        cnt--;
        LeftMessageUpdate();
    }

    private void LeftMessageUpdate()
    {
        leftText.SetText($"Left Tile ({cnt})");

        if (cnt == 0)
        {
            canDrag = false;
            Invoke("SetNextStage", 1.5f);
        }
    }
}
