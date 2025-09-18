using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CanvasFade : MonoBehaviour
{
    public static CanvasFade Instance;
    private const float loop = 0.1f;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void FadeCanvas(CanvasGroup _c, bool _o, float _t, UnityAction _e)
    {
        StartCoroutine(FadeCanvasCoroutine(_c, _o, _t, _e));
    }

    private IEnumerator FadeCanvasCoroutine(CanvasGroup _canvas, bool _open, float _totalTime, UnityAction _endEvent)
    {
        float _t = _totalTime * loop, _addValue = _open ? loop : -loop;

        _canvas.alpha = _open ? 0 : 1;

        while (_canvas.alpha != (_open ? 1 : 0))
        {
            yield return new WaitForSecondsRealtime(_t);
            _canvas.alpha += _addValue;
        }

        _endEvent?.Invoke();
    }
}
