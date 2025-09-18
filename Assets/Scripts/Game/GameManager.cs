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
    [SerializeField] private CustomBikeMovement _customBikeMovement;
    [SerializeField] private LeaderBoardController _leaderBoardController;

    [Header("Game Parameters")]
    [SerializeField] private int _maxPlayerHP = 3;
    [SerializeField] private float _timeBetweenDamage = 3f;
    [SerializeField] private float _sessionTime = 3 * 60;
    [SerializeField] private float _speedThresholdHigh = 15f;

    [Header("Collision")]
    [SerializeField] private string _obstacleTag = "Obstable";

    [Header("Events")]
    public UnityEvent OnGameStarted;
    public UnityEvent OnNewGameStarted;
    public UnityEvent<bool> OnGameViewChanged;
    public UnityEvent OnGameOver;
    public UnityEvent OnPlayerInHighSpeed;
    public UnityEvent OnPlayerInLowSpeed;

    private bool _leftHandTrigger = false;
    private bool _rightHandTrigger = false;

    private GameState _gameState;
    private bool _canTakeDamage = true;
    private bool _inGaming = false;
    private bool _isRunning = false;
    private bool _inHighSpeed = false;

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
            _inHighSpeed = false;
            _isRunning = true;
            _gameState = new GameState(_maxPlayerHP, _sessionTime);
            _uiGameHUDController.UpdateHUD(_gameState);

            OnNewGameStarted?.Invoke();
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
                _inHighSpeed = false;
                _customBikeMovement.ResetSpeedMultiplier();
                OnPlayerInLowSpeed?.Invoke();

                _uiGameHUDController.StartTakeDamageAnimation(null);
                _uiGameHUDController.UpdateHUD(_gameState);

                _canTakeDamage = false;

                StartCoroutine(WaitForSeconds(_timeBetweenDamage, () => _canTakeDamage = true));
            }
            else
            {
                HandleGameOver();
            }

        }else if (gameObject.TryGetComponent(out CoinCollectableController cointController))
        {
            _gameState.AddScore(cointController.Value);
            _uiGameHUDController.UpdateHUD(_gameState);

        }else if (gameObject.TryGetComponent(out SpeedMultiplierCollectableController speedMultiplierController))
        {
            Debug.Log($"[{GetType()}][HandlePlayerCollision] Speed Multiplier Collected = {speedMultiplierController.SpeedMultiplier}");

            _customBikeMovement.AddMultiplier(speedMultiplierController.SpeedMultiplier);

            if (_customBikeMovement.Speed>= _speedThresholdHigh && !_inHighSpeed)
            {
                OnPlayerInHighSpeed?.Invoke();
                _inHighSpeed = true;
            }
        }

        var collectable = gameObject.GetComponent<ICollectable>();
        collectable?.HandleObjectCollected();
    }

    private IEnumerator WaitForSeconds(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);

        callback?.Invoke();
    }

    private void HandleGameOver()
    {
        _isRunning = false;
        _inHighSpeed = false;
        _customBikeMovement.ResetSpeedMultiplier();
        OnGameOver?.Invoke();

        _uiGameHUDController.StartGameOverAnimation(_gameState, () =>
        {
            _uiGameHUDController.SetActive(false);
            _uiGameController.SetActive(true);

            _leaderBoardController.AddEntry(_gameState.Score, "Player");
        });
    }

    public void ToggleGameView()
    {
        if (_resetPlayerController.InResetProcess) return;

        _inGaming = !_inGaming;

        _uiGameController.SetActive(_inGaming);
        _uiGameHUDController.SetActive(_inGaming);
        _customBikeMovement.ResetSpeedMultiplier();

        OnGameViewChanged?.Invoke(_inGaming);
    }
}
