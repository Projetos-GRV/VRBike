using System;
using UnityEngine;

public interface IUIItemComponent
{
    void HandleItemComponent(object item, Action<object, GameObject> onClickCallback);
}
