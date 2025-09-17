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

    private Coroutine _waitAndActiveCoroutine;

    public void StartWaiting()
    {
        StopWaiting();

        StartCoroutine(WaitAndActiveCoroutine(_timeToWait, () => OnActive?.Invoke()));
    }

    public void StopWaiting()
    {
        if (_waitAndActiveCoroutine != null) StopCoroutine(_waitAndActiveCoroutine);
    }

    public void SetWaitingState(bool isWaiting)
    {
        if (isWaiting)
            StartWaiting();
        else
            StopWaiting();
    }


    private IEnumerator WaitAndActiveCoroutine(float timeToWait, Action callback)
    {
        yield return new WaitForSeconds(timeToWait);
        callback?.Invoke();
    }
}
