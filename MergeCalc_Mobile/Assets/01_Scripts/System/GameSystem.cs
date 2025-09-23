using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameSystem : MonoBehaviour
{
    protected MainTile mainTile;
    [SerializeField] private MainTile mainTilePrefab;

    [SerializeField] private Transform tileGroupTrm;
    [SerializeField] protected TextMeshProUGUI scoreTmp;
    protected int moveTileCnt, checkTileCnt, score, initValue = 1;
    
    [SerializeField] protected float tileMoveTime = 0.25f, boundValue = 0.1f;
    protected UnityEvent dragEvent;

    protected Queue<TileData> nextTiles = new();
    protected int showTileNum = 0;
    [SerializeField] protected List<ShowTile> showTiles;

    private Board board;

    private void Awake()
    {
        dragEvent = new();
        board = GetComponent<Board>();
    }

    protected virtual void Start()
    {
        mainTile = Instantiate(mainTilePrefab, tileGroupTrm);
        mainTile.Init(new TileData(CalcEnum.Plus, initValue, tileMoveTime, boundValue), MoveFinCheck);
        mainTile.mergeEvent.AddListener(OnMergeEvent);

        score = 0;
        scoreTmp.SetText("0");

        board.Init(mainTile, Move);
    }

    protected virtual void OnMergeEvent(int _n) => SoundSystem.Instance.UseEffectSound(EffectAudioType.Merge);

    protected virtual void GameOver()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game Over");
    }

    protected virtual void AddScore(int _value = 1)
    {
        score += _value;
        scoreTmp.SetText(score.ToString());
    }

    #region Tile
    protected abstract void SetNextTiles();

    protected void UpdateUiTiles()
    {
        foreach (var _tile in showTiles)
        {
            if(_tile.DataEmpty())
            {
                if (nextTiles.Count == 0) return;

                _tile.Init(nextTiles.Dequeue());
            }
        }
    }

    protected virtual void Spawn()
    {
        MoveTile tile = TilePooling.Instance.Pop(tileGroupTrm);
        tile.Init(showTiles[showTileNum].Use(), MoveFinCheck);

        board.Spawn(tile);

        showTileNum += 1;
        if (showTileNum == showTiles.Count) showTileNum = 0;

        UpdateUiTiles();
    }
    #endregion

    #region Move
    private void MoveFinCheck()
    {
        if (++checkTileCnt == moveTileCnt)
        {
            Spawn();
            UpdateUiTiles();
            dragEvent.Invoke();
        }
    }

    private void Move(List<MoveTile> _moveTiles)
    {
        moveTileCnt = _moveTiles.Count;
        checkTileCnt = 0;

        foreach (MoveTile item in _moveTiles) item.Move();
    }
    #endregion

    #region Quit
    protected virtual void OnDestroy()
    {
        dragEvent.RemoveAllListeners();
    }

    private void OnApplicationQuit()
    {
        ApplicationQuit();
    }

    protected virtual void ApplicationQuit() { }
    #endregion
}
