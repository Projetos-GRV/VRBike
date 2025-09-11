using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIGameHUDController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private GameObject _gameView;
    [SerializeField] private GameObject _startGameView;

    [SerializeField] private Transform _hpParent;
    [SerializeField] private Transform _hpPrefab;
    [SerializeField] private TextMeshProUGUI _txtScore;

    [SerializeField] private string _scoreFormat = "00000";

    [SerializeField] private Animator _animator;

    public UnityEvent OnStartGameAnimation;

    public void SetActive(bool isActive) => _view.SetActive(isActive);

    public void UpdateHUD(GameState gameState)
    {
        _hpParent.ClearChilds();

        for (int i = 0; i < gameState.CurrentPlayerHP; i++)
        {
            Instantiate(_hpPrefab, _hpParent);
        }

        _txtScore.text = gameState.Score.ToString(_scoreFormat);
    }

    public void StartGameAnimation()
    {
        _gameView.SetActive(false);
        _startGameView.SetActive(true);

        _animator.SetTrigger("Start");

        OnStartGameAnimation?.Invoke();

        StartCoroutine(WaitForSeconds(5, () =>
        {
            _gameView.SetActive(true);
            _startGameView.SetActive(false);
        }));
    }

    private IEnumerator WaitForSeconds(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);

        callback?.Invoke();
    }
}
