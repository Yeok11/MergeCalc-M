using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;

public class Home : MonoBehaviour
{
    [SerializeField] private Transform mainTile, basePos, leftPos, rightPos, upPos;
    [SerializeField] private DragSystem dragEvent;
    [SerializeField] private float t = 0.1f;

    [SerializeField] private List<ModeSO> modes;
    private int modeNum = 0;

    [SerializeField] private TextMeshProUGUI[] upTmpLine, middleTmpLine, underTmpLine;

    private void Start()
    {
        GameData.Init();

        dragEvent.dragEvent += TileMove;

        modeNum = 0;

        TitleUpdate();

        Invoke("MoveToBase", 0.05f);
    }

    public void ModeChange(Dir _dir)
    {
        if (_dir == Dir.Left) modeNum--;
        else if (_dir == Dir.Right) modeNum++;

        if (modeNum == modes.Count) modeNum = 0;
        else if (modeNum < 0) modeNum += modes.Count;

        TitleUpdate();
    }

    #region Move
    private void MoveToBase() => MoveToBase(0);

    private void MoveToBase(float _t)
    {
        mainTile.DOMove(basePos.position, _t);
    }

    private void TileMove(Dir _dir)
    {
        Sequence seq = DOTween.Sequence();

        switch (_dir)
        {
            case Dir.Up:
                seq.Append(mainTile.DOMove(upPos.position, t));
                seq.AppendCallback(ModeAction);
                return;

            case Dir.Left:
                seq.Append(mainTile.DOMove(leftPos.position, t));
                seq.AppendCallback(() => ModeChange(Dir.Left));
                break;

            case Dir.Right:
                seq.Append(mainTile.DOMove(rightPos.position, t));
                seq.AppendCallback(() => ModeChange(Dir.Right));
                break;
        }

        seq.AppendInterval(0.1f);
        seq.AppendCallback(MoveToBase);
    }
    #endregion

    private ModeSO GetMode() => modes[modeNum];

    public void TitleUpdate()
    {
        var _mode = GetMode();
        LineTextUpdate(1, _mode.firLineMes);
        LineTextUpdate(2, _mode.secLineMes);

        var _mes = "";

        if(_mode.showScore)
        {
            _mes = GameData.GetHScore(_mode.mode).ToString("D5");
        }

        LineTextUpdate(3, _mes);
    }

    private TextMeshProUGUI[] GetTmpLine(int _line)
    {
        if (_line == 1) return upTmpLine;
        else if (_line == 2) return middleTmpLine;
        else if (_line == 3) return underTmpLine;

        Debug.LogError("Line의 범위를 넘어섰습니다. (범위 : 1~3)");
        return null;
    }

    private void LineTextUpdate(int _line, string _txt)
    {
        var tmp = GetTmpLine(_line);

        for (int i = 0; i < tmp.Length; i++)
        {
            if (_txt.Length <= i) tmp[i].SetText("");
            else tmp[i].SetText(_txt[i].ToString());
        }
    }

    public void ModeAction()
    {
        switch (GetMode().mode)
        {
            case Mode.Live:
                SceneManager.LoadScene("Mode-Live");
                break;
            case Mode.Reach:
                SceneManager.LoadScene("Mode-Reach");
                break;
            case Mode.Explain:
                break;
            case Mode.Setting:
                break;
            case Mode.Quit:
                break;
        }
    }
}
