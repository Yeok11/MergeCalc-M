using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    public int num;
    public Calculate calc;
    [SerializeField] private float t = 0.4f;
    protected TextMeshProUGUI tmp;
    protected UnityAction moveFin;
    private Transform destination;

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    public virtual void Init(int _v, UnityAction _moveFin)
    {
        num = Random.Range(0, _v) + 1;
        calc = (Calculate)Random.Range(0, 4);
        moveFin = _moveFin;

        TextUpdate();

        gameObject.SetActive(true);
    }

    protected virtual void TextUpdate()
    {
        string str = "";
        switch (calc)
        {
            case Calculate.Plus: str = "+"; break;
            case Calculate.Minus: str = "-"; break;
            case Calculate.Multiple: str = "x"; break;
            case Calculate.Divide: str = "/"; break;
        }
        tmp.text = str + num.ToString();
    }

    public void MoveSet(Transform _trm)
    {
        destination = _trm;
    }

    public void Move()
    {
        transform.DOMove(destination.position, t).OnComplete(() => moveFin?.Invoke());
    }
}
