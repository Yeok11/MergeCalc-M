using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Tile mainTile;
    [SerializeField] private Transform tilePos;
    private int limitCnt = 5;

    private Board board;

    private void Awake()
    {
        board = GetComponent<Board>();
    }

    private void Start()
    {
        Tile m = Instantiate(mainTile, tilePos);
        m.Init(0);
        board.Init(m);
    }

    public void Spawn()
    {
        Tile t = TilePooling.Instance.Pop(tilePos);
        t.Init(limitCnt);

        board.Spawn(t);
    }

}
