using System.Collections.Generic;
using UnityEngine;

public class TilePooling : MonoBehaviour
{
    public static TilePooling Instance;

    [SerializeField] private MoveTile tilePrefab;
    private List<MoveTile> tileList = new List<MoveTile>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        for (int i = 0; i < 24; i++)
        {
            MoveTile t = Instantiate(tilePrefab, transform);
            Push(t);
        }
    }

    public void Push(MoveTile _tile)
    {
        _tile.transform.SetParent(transform);
        _tile.gameObject.SetActive(false);
        tileList.Add(_tile);
    }

    public MoveTile Pop(Transform _parent)
    {
        MoveTile t = tileList[0];
        tileList.RemoveAt(0);
        t.transform.SetParent(_parent);
        t.transform.localScale = Vector3.one;

        return t;
    }
}
