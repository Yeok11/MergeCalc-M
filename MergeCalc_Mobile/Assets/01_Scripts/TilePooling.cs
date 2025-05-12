using System.Collections.Generic;
using UnityEngine;

public class TilePooling : MonoBehaviour
{
    public static TilePooling Instance;

    [SerializeField] private Tile tilePrefab;
    private List<Tile> tileList = new List<Tile>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        for (int i = 0; i < 24; i++)
        {
            Tile t = Instantiate(tilePrefab, transform);
            Push(t);
        }
    }

    public void Push(Tile _tile)
    {
        _tile.transform.parent = transform;
        _tile.gameObject.SetActive(false);
        tileList.Add(_tile);
    }

    public Tile Pop(Transform _parent)
    {
        Tile t = tileList[0];
        tileList.RemoveAt(0);
        t.transform.parent = _parent;
        t.transform.localScale = Vector3.one;

        return t;
    }
}
