using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitToActive : MonoBehaviour
{
    [SerializeField] private float _timeToWait = 3f;

    [Header("Events")]
    public UnityEvent OnActive;
    public UnityEvent OnStartWaiting;
    public UnityEvent OnStoptWaiting;

    private Coroutine _waitAndActiveCoroutine;


    public void StartWaiting()
    {
        StopWaiting();

        _waitAndActiveCoroutine = StartCoroutine(WaitAndActiveCoroutine(_timeToWait, () => OnActive?.Invoke()));
    }

    public void StopWaiting(bool notify=false)
    {
        if (_waitAndActiveCoroutine != null) StopCoroutine(_waitAndActiveCoroutine);
        
        if (notify) OnStoptWaiting?.Invoke();
    }

    public void SetWaitingState(bool isWaiting)
    {
        if (isWaiting)
            StartWaiting();
        else
            StopWaiting(true);

        Debug.Log($"SetWaitingState  {isWaiting}");
    }

    private IEnumerator WaitAndActiveCoroutine(float timeToWait, Action callback)
    {
        OnStartWaiting?.Invoke();

        yield return new WaitForSeconds(timeToWait);
        callback?.Invoke();
    }
}
