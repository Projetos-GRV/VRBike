using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private float _sessionTime = 3 * 60;

    [Header("Collision")]
    [SerializeField] private string _obstacleTag = "Obstable";

    [Header("Events")]
    public UnityEvent OnGameStarted;

    private bool _leftHandTrigger = false;
    private bool _rightHandTrigger = false;

    private GameState _gameState;
    private bool _canTakeDamage = true;
    private bool _inGaming = false;
    private bool _isRunning = false;


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

        _inGaming = false;
        _isRunning = false;
        _uiGameHUDController.SetActive(false);
        _uiGameController.SetActive(false);
    }

    private void Start()
    {
        OnGameStarted?.Invoke();
    }

    private void Update()
    {
        if (_isRunning && _inGaming)
        {
            _uiGameHUDController.UpdateTime(_gameState);

            if (_gameState.CurrentTimeNormalized >= 1f)
            {
                HandleGameOver();
            }
        }
    }

    private void SetHandTrigger(ref bool variable, bool newValue)
    {
        variable = newValue;

        if (_leftHandTrigger && _rightHandTrigger)
        {
            if (_inGaming)
            {
                StartNewGame();
            }
            else
            {
                _resetPlayerController.StartResetProcess(null, null);
            }
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
        _uiGameHUDController.StartGameAnimation(() =>
        {
            _isRunning = true;
            _gameState = new GameState(_maxPlayerHP, _sessionTime);
            _uiGameHUDController.UpdateHUD(_gameState);
        });
    }

    private void HandlePlayerCollision(GameObject gameObject)
    {
        if (!_isRunning || !_inGaming) return;

        if (gameObject.tag.Equals(_obstacleTag) && _canTakeDamage)
        {
            _gameState.TakeDamege();

            if (_gameState.IsPlayerAlive())
            {
                _uiGameHUDController.StartTakeDamageAnimation(null);
                _uiGameHUDController.UpdateHUD(_gameState);

                _canTakeDamage = false;

                StartCoroutine(WaitForSeconds(_timeBetweenDamage, () => _canTakeDamage = true));
            }
            else
            {
                HandleGameOver();
            }
        }
    }

    private IEnumerator WaitForSeconds(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);

        callback?.Invoke();
    }

    private void HandleGameOver()
    {
        _isRunning = false;

        _uiGameHUDController.StartGameOverAnimation(() =>
        {
            _uiGameHUDController.SetActive(false);
            _uiGameController.SetActive(true);
        });
    }

    public void ToggleGameView()
    {
        _inGaming = !_inGaming;

        _uiGameController.SetActive(_inGaming);
        _uiGameHUDController.SetActive(_inGaming);
    }
}
