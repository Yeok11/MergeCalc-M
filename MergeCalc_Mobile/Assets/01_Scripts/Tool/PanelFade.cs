using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PanelFade : MonoBehaviour
{
    public static PanelFade Instance;
    private const float loop = 0.1f;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void FadePanel(Panel _panel, bool _isOpen, float _time, UnityEvent _event) => StartCoroutine(FadePanelCoroutine(_panel, _isOpen, _time, _event));

    private IEnumerator FadePanelCoroutine(Panel _panel, bool _isOpen, float _time, UnityEvent _endEvent)
    {
        float _t = _time * loop, _addValue = _isOpen ? loop : -loop;

        var _canvas = _panel.GetCanvas();
        _canvas.alpha = _isOpen ? 0 : 1;

        bool _openState = _isOpen;

        while (_canvas.alpha != (_isOpen ? 1 : 0) && _panel.isOpenState == _isOpen)
        {
            yield return new WaitForSecondsRealtime(_t);
            _openState = _panel.isOpenState;
            if (_openState != _isOpen) break;

            _canvas.alpha += _addValue;
        }

        if(_openState == _isOpen) _endEvent?.Invoke();
    }
}
