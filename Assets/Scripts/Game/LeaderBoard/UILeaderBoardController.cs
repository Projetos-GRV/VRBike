using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILeaderBoardController : MonoBehaviour
{
    [SerializeField] private UIListDisplayController _rowsListDisplayController;

    public void UpdateUI(LeaderboardData leaderboardData)
    {
        _rowsListDisplayController.Init(leaderboardData.Entries, null);
    }
}
