using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public int MaxPlayerHP;
    public int CurrentPlayerHP;
    public int Score;
    public float CurrentTime;
    public float MaxTime;

    private float _firstTim;

    public GameState(int maxPlayerHP, float maxTime)
    {
        MaxPlayerHP = maxPlayerHP;
        CurrentPlayerHP = MaxPlayerHP;
        Score = 0;
        CurrentTime = 0;
        MaxTime = maxTime;

        _firstTim = Time.time;
    }

    public void TakeDamege()
    {
        CurrentPlayerHP = Mathf.Max(CurrentPlayerHP - 1, 0);
    }

    public void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;
    }

    public float CurrentTimeNormalized => (Time.time - _firstTim) / MaxTime;

    public bool IsPlayerAlive() => CurrentPlayerHP > 0;
}
