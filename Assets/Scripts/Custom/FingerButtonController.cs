using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FingerButtonController : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnFingerEnter;
    public UnityEvent OnFingerExit;

    [Header("Visuals")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _colorActivated;

    [Header("Parameters")]
    [SerializeField] private float _timeBetweenEvents = 1f;
    [SerializeField] private bool _isToggle = false;
    [SerializeField] private bool _isOn = false;

    private bool _canInteract = true;
    private Coroutine _waitSecondsCoroutine;
    private Color _defaultColor;

    private void Awake()
    {
        _defaultColor = _renderer.material.color;

        if (_isToggle && _isOn)
        {
            SetButtonActivated();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("FingerInteractor") || !_canInteract) return;
        
        OnFingerEnter?.Invoke();

        if (_waitSecondsCoroutine != null)
            StopCoroutine(_waitSecondsCoroutine);

        _canInteract = false;
        _waitSecondsCoroutine = StartCoroutine(WaitSecondsCoroutine(_timeBetweenEvents, () => _canInteract = true));

        if (_isToggle)
        {
            _isOn = !_isOn;

            if (_isOn)
            {
                SetButtonActivated();
            }
            else
            {
                SetButtonDeactivated();
            }
        }
        else
        {
            SetButtonActivated();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.tag.Equals("FingerInteractor")) return;

        OnFingerExit?.Invoke();

        if (!_isToggle)
            SetButtonDeactivated();
    }

    private IEnumerator WaitSecondsCoroutine(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);

        callback?.Invoke();
    }

    private void SetButtonActivated()
    {
        _renderer.material.color = _colorActivated;
    }

    private void SetButtonDeactivated()
    {
        _renderer.material.color = _defaultColor;
    }

    public void SetActiveWithoutNotify(bool isContantSpeedEnabled)
    {
        if (_isToggle)
        {
            _isOn = !_isOn;

            if (_isOn)
            {
                SetButtonActivated();
            }
            else
            {
                SetButtonDeactivated();
            }
        }
        else
        {
            SetButtonActivated();
        }
    }
}
