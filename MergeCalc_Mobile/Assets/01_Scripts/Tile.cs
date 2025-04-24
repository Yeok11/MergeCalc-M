using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    public int num;
    public Calculate calc;
    [SerializeField] private float t = 0.4f;

    public void Init(int _v)
    {
        num = Random.Range(-_v, _v + 1);
    }

    public void Move(Transform _trm)
    {
        transform.DOMove(_trm.position, t);
    }
}
