using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppSettingsManager : MonoBehaviour, ISettingsReceiver
{
    private const string EVENT_RESET = "resetbike";
    private const string EVENT_RESET_PLAYER = "resetplayer";
    private const string EVENT_GAME = "game";
    private const string EVENT_AUTO = "auto";
    private const string EVENT_RESET_LEADERBOARD = "resetboard";
    private const string EVENT_SPEED_MULTIPLIER = "speedmultiplier";

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private ResetPlayerPositionController _resetPlayerPositionController;
    [SerializeField] private CustomBikeMovimentSource _customBikeMovimentSource;
    [SerializeField] private UISpeedometerController _uiSpeedometerController;

    public List<string> EventsToListen()
    {
        return new List<string> { EVENT_RESET, EVENT_GAME, EVENT_RESET_LEADERBOARD, EVENT_RESET_PLAYER, EVENT_SPEED_MULTIPLIER, EVENT_AUTO };
    }

    public void HandleEvent(string eventID, string argType, string argValue)
    {
        switch (eventID)
        {
            case EVENT_RESET: HandleResetEvent(); break;
            case EVENT_RESET_PLAYER: HandleResetPlayer(); break;
            case EVENT_GAME: HandleGameEvent(); break;
            case EVENT_RESET_LEADERBOARD: HandleResetLeaderboard(); break;
            case EVENT_SPEED_MULTIPLIER: HandleSpeedMultiplierEvent(argValue) ; break;
            case EVENT_AUTO: HandleAutoEvent(); break;
            default: break;
        }
    }

    private void HandleResetEvent()
    {
        _gameManager.HandleResetEvent();
    }

    private void HandleGameEvent()
    {
        _gameManager.ToggleGameView();
    }

    private void HandleResetLeaderboard()
    {
        _gameManager.ResetLeaderBoard();
    }

    private void HandleResetPlayer()
    {
        _resetPlayerPositionController.ResetPlayerPosition();
    }

    private void HandleSpeedMultiplierEvent(string strSpeedMultiplier)
    {
        _customBikeMovimentSource.HandleSpeedMultplierChanged(strSpeedMultiplier);
    }

    private void HandleAutoEvent()
    {
        _uiSpeedometerController.ToggleConstantSpeed();
    }
}
