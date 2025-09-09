using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIConfirmationScreenController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _uiView;
    [SerializeField] private Transform _hmdReference;
    [SerializeField] private Slider _sliderTimer;
    [SerializeField] private TextMeshProUGUI _txtDescription;

    [Header("Parameters")]
    [SerializeField] private bool _leftToRight;
    [SerializeField] private float _distance = 1.5f;

    private Action _onConfirmaton;
    private Action _onCancel;
    private Coroutine _animateSliderCoroutine;

    private void Awake()
    {
        _uiView.SetActive(false);
    }

    public void GetConfirmation(string description, float secondsToRespond, Action OnConfirmation, Action OnCancel)
    {
        _onConfirmaton = OnConfirmation;
        _onCancel = OnCancel;
        
        _uiView.SetActive(true);
        _txtDescription.text = description;

        _uiView.transform.SetPositionInFrontOf(_hmdReference, _distance);

        if (secondsToRespond > 0)
        {
            if (_animateSliderCoroutine != null)
                StopCoroutine(_animateSliderCoroutine);

            _animateSliderCoroutine = StartCoroutine(AnimeteSliderCoroutine(secondsToRespond, _leftToRight, Cancel));
        }
    }

    public float seconds = 3;
    private void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            Debug.Log("T");
            GetConfirmation("Teste?", seconds, () => Debug.Log("Tecla T"), null);
        }


        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            Confirm();
        }

        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            Cancel();
        }
    }

    private IEnumerator AnimeteSliderCoroutine(float totalTime, bool leftToRight, Action onCompleted)
    {
        var initialValue = leftToRight ? 0 : totalTime;
        var finalValue = leftToRight ? totalTime : 0;
        var currentValue = initialValue;
        var signal = leftToRight ? 1f : -1f;

        _sliderTimer.minValue = Mathf.Min(initialValue, finalValue);
        _sliderTimer.maxValue = Mathf.Max(initialValue, finalValue);
        _sliderTimer.value = 0;

        while (true)
        {
            Debug.Log($"{initialValue} {finalValue} {currentValue} {signal}");
            _sliderTimer.value = currentValue;

            currentValue += Time.deltaTime * signal;

            yield return null;

            if (Mathf.Abs(currentValue - finalValue) <= 0.01f) break;
        }

        onCompleted?.Invoke();
    }

    public void Confirm()
    {
        if (_animateSliderCoroutine != null) StopCoroutine(_animateSliderCoroutine);

        _onConfirmaton?.Invoke();

        _uiView.SetActive(false);
    }

    public void Cancel()
    {
        if (_animateSliderCoroutine != null) StopCoroutine(_animateSliderCoroutine);

        _onCancel?.Invoke();

        _uiView.SetActive(false);
    }
}
