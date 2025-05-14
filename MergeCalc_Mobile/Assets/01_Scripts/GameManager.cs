using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private PlayerTile mainTile;
    [SerializeField] private Transform tilePos;
    private int limitCnt = 5;

    private Board board;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        board = GetComponent<Board>();
    }

    private void Start()
    {
        PlayerTile _mainTile = Instantiate(mainTile, tilePos);
        _mainTile.Init(1);
        board.Init(_mainTile);
    }

    public void Spawn()
    {
        Tile tile = TilePooling.Instance.Pop(tilePos);
        tile.Init(limitCnt);

        board.Spawn(tile);
    }

}
