using UnityEngine;
using DG.Tweening;
using TMPro;

public class Tile : MonoBehaviour
{
    public int num;
    public Calculate calc;
    [SerializeField] private float t = 0.4f;
    protected TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    public virtual void Init(int _v)
    {
        num = Random.Range(0, _v) + 1;
        calc = (Calculate)Random.Range(0, 4);

        string str = "";
        switch (calc)
        {
            case Calculate.Plus:    str = "+"; break;
            case Calculate.Minus:   str = "-"; break;
            case Calculate.Multiple:str = "x"; break;
            case Calculate.Divide:  str = "/"; break;
        }
        str += num.ToString();

        tmp.text = str;
    }

    public void Move(Transform _trm)
    {
        transform.DOMove(_trm.position, t);
    }
}
