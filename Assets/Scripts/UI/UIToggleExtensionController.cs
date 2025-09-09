using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UIToggleExtensionController : MonoBehaviour
{
    public UnityEvent OnToogleOn;
    public UnityEvent OnToggleOff;

    private void Awake()
    {
        var toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(HandleToggleChanged);
    }

    public void HandleToggleChanged(bool newValue)
    {
        if (newValue)
        {
            OnToogleOn?.Invoke();
        }
        else
        {
            OnToggleOff?.Invoke();
        }
    }
}
