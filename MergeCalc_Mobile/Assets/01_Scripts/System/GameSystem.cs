using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;

    protected MainTile mainTile;
    [SerializeField] private MainTile mainTilePrefab;
    [SerializeField] private Transform tilePos;
    [SerializeField] protected TextMeshProUGUI scoreTmp;
    protected int moveTileCnt, checkTileCnt, score, initValue = 1;

    protected Queue<TileData> nextTiles = new();
    protected int showTileNum = 0;
    [SerializeField] protected List<ShowTile> showTiles;
    [SerializeField] protected float tileMoveTime = 0.25f, boundValue = 0.1f;

    private Board board;
    
    protected UnityAction dragEvent;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        board = GetComponent<Board>();
    }

    protected virtual void Start()
    {
        mainTile = Instantiate(mainTilePrefab, tilePos);
        mainTile.Init(new TileData(CalcEnum.Plus, initValue, tileMoveTime, boundValue), MoveFinCheck);

        mainTile.mergeEvent += (int _n) => SoundSystem.Instance.UseEffectSound(EffectAudioType.Merge);

        score = 0;
        scoreTmp.SetText("0");

        board.Init(mainTile);
    }

    public virtual void GameOver()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game Over");
    }

    protected virtual void AddScore(int _value)
    {
        score += _value;
        scoreTmp.SetText(score.ToString());
    }

    #region Tile
    public abstract void SetNextTiles();

    protected void UpdateUiTiles()
    {
        foreach (var tile in showTiles)
        {
            if(tile.DataEmpty())
            {
                if (nextTiles.Count == 0) return;

                tile.Init(nextTiles.Dequeue());
            }
        }
    }

    protected virtual void Spawn()
    {
        MoveTile tile = TilePooling.Instance.Pop(tilePos);
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

    public void Move(List<MoveTile> _moveTiles)
    {
        moveTileCnt = _moveTiles.Count;
        checkTileCnt = 0;

        foreach (MoveTile item in _moveTiles) item.Move();
    }
    #endregion

    #region Option or Quit
    private void OnApplicationQuit()
    {
        ApplicationQuit();
    }

    protected virtual void ApplicationQuit() { }
    #endregion
}
