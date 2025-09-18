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
    [SerializeField] private CanvasGroup exitPanel;
    private bool openExitPanel;

    private ModeSO GetMode() => modes[modeNum];

    private void Start()
    {
        GameData.InitDrag();
        Time.timeScale = 1;

        dragEvent.dragEvent += TileMove;

        modeNum = (int)GameData.currentMode;

        TitleUpdate();

        mainTile.transform.localPosition = Vector3.zero;
        openExitPanel = false;
    }

    public void ModeChange(Direction _dir)
    {
        if (_dir == Direction.Left) modeNum--;
        else if (_dir == Direction.Right) modeNum++;

        if (modeNum == modes.Count) modeNum = 0;
        else if (modeNum < 0) modeNum += modes.Count;

        TitleUpdate();
    }

    public void ModeAction()
    {
        switch (GetMode().mode)
        {
            case Mode.Live:
                GameData.canDrag = false;
                SceneManager.LoadScene("Mode-Live");
                break;

            case Mode.Reach:
                GameData.canDrag = false;
                SceneManager.LoadScene("Mode-Reach");
                break;

            case Mode.Explain:
                break;

            case Mode.Setting:
                Option.Instance.Open();

                break;

            case Mode.Quit:
                Application.Quit();
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            openExitPanel = !openExitPanel;
            exitPanel.gameObject.SetActive(true);

            CanvasFade.Instance.FadeCanvas(exitPanel, openExitPanel, 0.01f, () =>
                {
                    if(!openExitPanel) exitPanel.gameObject.SetActive(false);
                });
        }
    }

    #region Move
    private void MoveToBase() => MoveToBase(0);

    private void MoveToBase(float _t)
    {
        mainTile.DOMove(basePos.position, _t);
    }

    private void TileMove(Direction _dir)
    {
        Sequence seq = DOTween.Sequence();

        switch (_dir)
        {
            case Direction.Up:
                seq.Append(mainTile.DOMove(upPos.position, t));
                seq.AppendCallback(ModeAction);
                break;

            case Direction.Left:
                seq.Append(mainTile.DOMove(leftPos.position, t));
                seq.AppendCallback(() => ModeChange(Direction.Left));
                break;

            case Direction.Right:
                seq.Append(mainTile.DOMove(rightPos.position, t));
                seq.AppendCallback(() => ModeChange(Direction.Right));
                break;
        }

        seq.AppendInterval(0.1f);
        seq.AppendCallback(MoveToBase);
    }
    #endregion

    #region Text
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
    #endregion
}
