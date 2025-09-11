using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIGameManagerController _uiGameController;
    [SerializeField] private UIGameHUDController _uiGameHUDController;
    [SerializeField] private HandGesturesManager _handGesturesManager;
    [SerializeField] private ResetPlayerController _resetPlayerController;

    private bool _leftHandTrigger = false;
    private bool _rightHandTrigger = false;

    private void Awake()
    {
        if (_handGesturesManager != null)
        {
            _handGesturesManager.OnLeftThumbsUpEvent.AddListener(value => SetHandTrigger(ref _leftHandTrigger, value));
            _handGesturesManager.OnRightThumbsUpEvent.AddListener(value => SetHandTrigger(ref _rightHandTrigger, value));
        }
    }

    private void SetHandTrigger(ref bool variable, bool newValue)
    {
        variable = newValue;

        if (_leftHandTrigger && _rightHandTrigger)
        {
            StartNewGame();
        }
    }

    public void StartNewGame()
    {
        _uiGameController.SetActive(false);
        _uiGameHUDController.SetActive(false);

        _resetPlayerController.StartResetProcess(HandleResetCompleted, null);
    }

    private void HandleResetCompleted()
    {
        _uiGameHUDController.SetActive(true);
    }
}
