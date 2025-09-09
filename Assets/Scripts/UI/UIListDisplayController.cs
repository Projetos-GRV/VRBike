
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIListDisplayController : MonoBehaviour
{
    [SerializeField] private RectTransform _itemParent;

    [SerializeField] private UIListItemTemplateDisplayController _itemPrefab;



    public void Init(int itemAmount)
    {
        _itemParent.ClearChilds();

        for (int i = 0; i < itemAmount; i++)
        {
            var instance = Instantiate(_itemPrefab, _itemParent);
        }
    }

    public void Init<T>(List<T> items, Action<object, GameObject> onItemSelected)
    {
        List<object> newItems = new List<object>();

        foreach (var item in items) newItems.Add(item);

        Init(newItems, onItemSelected);
    }

    public void Init(List<object> items, Action<object, GameObject> onItemSelected)
    {
        _itemParent.ClearChilds();

        foreach (var item in items)
        {
            var instance = Instantiate(_itemPrefab, _itemParent);

            instance.Init(item, onItemSelected);
        }
    }

    public List<GameObject> GetItems()
    {
        return _itemParent.GetChilds();
    }

    public void Clear()
    {
        _itemParent.ClearChilds();
    }
}
