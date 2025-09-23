using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    private Dictionary<PanelType, Panel> panelDic = new();
    [SerializeField] private Panel[] panels;

    [SerializeField] private GameObject blackView;
    [SerializeField] private bool pauseCheck;

    [SerializeField] private PanelType escapePanel;

    private bool openPanelState;
    private PanelType currentPanel;

    private void Awake()
    {
        foreach (var _panel in panels)
        {
            _panel.Init();
            _panel.onCloseEvent.AddListener(() => openPanelState = false);
            _panel.onCloseEvent.AddListener(() => blackView.SetActive(false));

            var _key = _panel.GetKey();

            if (panelDic.ContainsKey(_key))
                Debug.LogError($"This key is already contained in this Dictionary. Key -> {_key}");
            else
                panelDic.Add(_key, _panel);
        }

        blackView.SetActive(false);
    }

    public void OpenOption() => Open(PanelType.Option);

    public void Open(PanelType _type)
    {
        openPanelState = true;
        blackView.SetActive(true);
        currentPanel = _type;
        panelDic[_type].Open();
    }

    public void Close()
    {
        panelDic[currentPanel].Close();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (openPanelState)
                Close();
            else
                Open(escapePanel);
        }
    }

    private void OnDisable()
    {
        foreach (var _panel in panels)
        {
            _panel.onOpenEvent.RemoveAllListeners();
            _panel.onCloseEvent.RemoveAllListeners();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pauseCheck) Open(escapePanel);
    }
}