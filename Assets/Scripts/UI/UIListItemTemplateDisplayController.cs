using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIListItemTemplateDisplayController : MonoBehaviour
{
    [SerializeField] private GameObject _aditionalComponentsParent;

    private object _item;
    private Action<object, GameObject> _onClick;

    public void Init(object item, Action<object, GameObject> onItemSelected)
    {
        _item = item;
        _onClick = onItemSelected;

        foreach (var component in _aditionalComponentsParent.GetComponents<IUIItemComponent>())
        {
            component.HandleItemComponent(item, onItemSelected);
        }
    }

    public void SelectItem()
    {
        _onClick?.Invoke(Item, Instance);
    }

    public object Item { get { return _item;  } }


    public GameObject Instance { get { return gameObject; } }
}
