using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEnvironmentItemController : MonoBehaviour, IUIItemComponent
{
    [SerializeField] private Image _imgEnvironment;
    [SerializeField] private TextMeshProUGUI _txtEnvironment;

    public void HandleItemComponent(object item, Action<object, GameObject> onClickCallback)
    {
        var environmentData = item as EnvironmentSO;

        _imgEnvironment.sprite = environmentData.Thumbnail;
        _txtEnvironment.text = environmentData.Name;
    }
}
