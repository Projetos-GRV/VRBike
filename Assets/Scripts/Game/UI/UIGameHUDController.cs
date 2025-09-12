using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIGameHUDController : MonoBehaviour
{
    [Header("Screen References")]
    [SerializeField] private GameObject _view;
    [SerializeField] private GameObject _gameView;
    [SerializeField] private GameObject _startGameView;
    [SerializeField] private GameObject _gameOverView;
    [SerializeField] private Animator _animator;

    [Header("Player Info")]
    [SerializeField] private Transform _hpParent;
    [SerializeField] private Transform _hpPrefab;
    [SerializeField] private TextMeshProUGUI _txtScore;
    [SerializeField] private UISliderTimer _sliderTimer;
    [SerializeField] private TextMeshProUGUI _txtTimer;

    [Header("Parameters")]
    [SerializeField] private string _scoreFormat = "00000";
    [SerializeField] private float _startAnimationDuration = 5f;
    [SerializeField] private float _gameOverAnimationDuration = 4.5f;
    [SerializeField] private float _takeDamageAnimationDuration = 0.83f;
    [SerializeField] private string _timerPrefix = "Tempo restante:\n";

    [Header("Events")]
    public UnityEvent OnStartGameAnimation;
    public UnityEvent OnStartGameOverAnimation;
    public UnityEvent OnStartTakeDamageAnimation;

    public void SetActive(bool isActive) => _view.SetActive(isActive);

    private void Awake()
    {
        _view.SetActive(false);

        _startGameView.SetActive(false);
        _gameOverView.SetActive(false);
        _gameView.SetActive(false);
    }

    public void UpdateHUD(GameState gameState)
    {
        _hpParent.ClearChilds();

        for (int i = 0; i < gameState.CurrentPlayerHP; i++)
        {
            Instantiate(_hpPrefab, _hpParent);
        }
        _txtScore.text = gameState.Score.ToString(_scoreFormat);

        UpdateTime(gameState);
    }

    public void StartGameAnimation(Action callback)
    {
        _gameView.SetActive(false);
        _startGameView.SetActive(true);
        _gameOverView.SetActive(false);

        _animator.SetTrigger("Start");

        OnStartGameAnimation?.Invoke();

        StartCoroutine(WaitForSeconds(_startAnimationDuration, () =>
        {
            _gameView.SetActive(true);
            _startGameView.SetActive(false);

            callback?.Invoke();
        }));
    }

    public void StartGameOverAnimation(Action callback)
    {
        _gameView.SetActive(false);
        _startGameView.SetActive(false);
        _gameOverView.SetActive(true);

        _animator.SetTrigger("GameOver");

        OnStartGameOverAnimation?.Invoke();

        StartCoroutine(WaitForSeconds(_gameOverAnimationDuration, () =>
        {
            callback?.Invoke();
        }));
    }

    public void StartTakeDamageAnimation(Action callback)
    {
        _animator.SetTrigger("TakeDamage");

        OnStartTakeDamageAnimation?.Invoke();

        StartCoroutine(WaitForSeconds(_takeDamageAnimationDuration, callback));
    }

    private IEnumerator WaitForSeconds(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);

        callback?.Invoke();
    }

    public void UpdateTime(GameState gameState)
    {
        _sliderTimer.UpdateTime(1 - gameState.CurrentTimeNormalized);

        var totalSeconds = gameState.MaxTime * (1 - gameState.CurrentTimeNormalized);

        int minutes = (int) totalSeconds / 60;
        int seconds = (int)totalSeconds % 60;

        Debug.Log($"{minutes}  {seconds}");
        _txtTimer.text = string.Format("{0}{1:00} : {2:00}", _timerPrefix, minutes, seconds);
    }
}
