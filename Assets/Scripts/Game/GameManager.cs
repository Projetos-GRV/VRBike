using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIGameManagerController _uiGameController;
    [SerializeField] private UIGameHUDController _uiGameHUDController;
    [SerializeField] private HandGesturesManager _handGesturesManager;
    [SerializeField] private ResetPlayerController _resetPlayerController;
    [SerializeField] private PlayerCollisionDetectorController _playerCollisionDetectorController;

    [Header("Game Parameters")]
    [SerializeField] private int _maxPlayerHP = 3;
    [SerializeField] private float _timeBetweenDamage = 3f;

    [Header("Collision")]
    [SerializeField] private string _obstacleTag = "Obstable";

    private bool _leftHandTrigger = false;
    private bool _rightHandTrigger = false;

    private GameState _gameState;
    private bool _canTakeDamage = true;

    private void Awake()
    {
        if (_handGesturesManager != null)
        {
            _handGesturesManager.OnLeftThumbsUpEvent.AddListener(value => SetHandTrigger(ref _leftHandTrigger, value));
            _handGesturesManager.OnRightThumbsUpEvent.AddListener(value => SetHandTrigger(ref _rightHandTrigger, value));
        }

        if (_playerCollisionDetectorController != null)
        {
            _playerCollisionDetectorController.OnPlayerCollideWithObject.AddListener(HandlePlayerCollision);
        }

        _uiGameHUDController.SetActive(false);
        _uiGameController.SetActive(true);
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
        _gameState = new GameState(_maxPlayerHP, 0);

        _uiGameHUDController.SetActive(true);
        _uiGameHUDController.UpdateHUD(_gameState);
        _uiGameHUDController.StartGameAnimation();
    }

    private void HandlePlayerCollision(GameObject gameObject)
    {
        if (gameObject.tag.Equals(_obstacleTag) && _canTakeDamage)
        {
            _gameState.TakeDamege();
            _uiGameHUDController.UpdateHUD(_gameState);

            _canTakeDamage = false;

            StartCoroutine(WaitForSeconds(_timeBetweenDamage, () => _canTakeDamage = true));
        }
    }

    private IEnumerator WaitForSeconds(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);

        callback?.Invoke();
    }
}
