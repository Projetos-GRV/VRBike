using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDebbugerController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private TextMeshProUGUI _txtLog;

    private void Awake()
    {
        Application.logMessageReceived += Application_logMessageReceived;
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        _txtLog.text += $"[{type}]{condition}\n";
    }

    public void ToggleVisibity(bool value)
    {
        if (value)
        {
            _view.SetActive(!_view.activeSelf);
        }
    }
}
