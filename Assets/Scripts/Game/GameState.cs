using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public int MaxPlayerHP;
    public int CurrentPlayerHP;
    public int Score;

    public GameState(int maxPlayerHP, int score)
    {
        MaxPlayerHP = maxPlayerHP;
        CurrentPlayerHP = MaxPlayerHP;
        Score = score;
    }

    public void TakeDamege()
    {
        CurrentPlayerHP = Mathf.Max(CurrentPlayerHP - 1, 0);
    }

    public void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;
    }

    public bool IsPlayerAlive() => CurrentPlayerHP > 0;
}
