using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandGesturesManager : MonoBehaviour
{
    public UnityEvent<bool> OnRightThumbsUpEvent;
    public UnityEvent<bool> OnRightPalmUpEvent;
    public UnityEvent<bool> OnRightFistEvent;

    public UnityEvent<bool> OnLeftPalmUpEvent;
    public UnityEvent<bool> OnLeftThumbsUpEvent;
    public UnityEvent<bool> OnLeftFistEvent;

    public void TriggerRightThumbsUpEvent(bool value) => OnRightThumbsUpEvent?.Invoke(value);
    public void TriggerRightPalmUpEvent(bool value) => OnRightPalmUpEvent?.Invoke(value);
    public void TriggerRighFistEvent(bool value) => OnRightFistEvent?.Invoke(value);

    public void TriggerLeftPalmUpEvent(bool value) => OnLeftPalmUpEvent?.Invoke(value);
    public void TriggerLeftThumbsUpEvent(bool value) => OnLeftThumbsUpEvent?.Invoke(value);
    public void TriggerLeftFistEvent(bool value) => OnLeftFistEvent?.Invoke(value);
}
