using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class LeaderBoardController : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private int _maxEntries = 5;   // Máximo de registros
    [SerializeField] private bool _allowNewRecords = true; // Se registros são permitidos

    [Header("Events")]
    public UnityEvent<LeaderboardData> OnEntriesChanged;

    private LeaderboardData _leaderboardData = new LeaderboardData();
    private string _savePath;

    /// <summary>
    /// Retorna a lista atual do leaderboard
    /// </summary>
    public List<LeaderboardEntry> Entries => _leaderboardData.Entries;

    private void Awake()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        LoadLeaderboard();
        FillEmptyEntries();
    }

    private void Start()
    {
        OnEntriesChanged?.Invoke(_leaderboardData);
    }

    /// <summary>
    /// Define se novos registros são permitidos
    /// </summary>
    public void SetAllowNewRecords(bool allow)
    {
        _allowNewRecords = allow;
    }

    /// <summary>
    /// Verifica se um score pode entrar no leaderboard
    /// </summary>
    public bool CanEnterLeaderboard(int score)
    {
        if (!_allowNewRecords) return false;

        // Sempre cabe se ainda não preencheu tudo
        if (_leaderboardData.Entries.Count < _maxEntries) return true;

        // Caso contrário, só entra se for maior que o menor
        return score > _leaderboardData.Entries[_maxEntries - 1].Score;
    }

    public int GetPositionForScore(int score)
    {
        if (!_allowNewRecords) return -1;

        // Sempre cabe se ainda não preencheu tudo
        if (_leaderboardData.Entries.Count < _maxEntries) return -1;

        for (int i = 0; i < _leaderboardData.Entries.Count; i++)
        {
            if (score > _leaderboardData.Entries[i].Score) return i + 1;
        }

        return -1;
    }

    /// <summary>
    /// Adiciona um novo registro ao leaderboard (se permitido)
    /// </summary>
    public bool AddEntry(int score, string playerName)
    {
        if (!CanEnterLeaderboard(score)) return false;

        LeaderboardEntry newEntry = new LeaderboardEntry(0, score, playerName);

        _leaderboardData.Entries.Add(newEntry);
        _leaderboardData.Entries.Sort((a, b) => b.Score.CompareTo(a.Score));

        // Ajustar ranks
        for (int i = 0; i < _leaderboardData.Entries.Count; i++)
        {
            _leaderboardData.Entries[i].Rank = i + 1;
        }

        // Manter apenas o limite
        if (_leaderboardData.Entries.Count > _maxEntries)
            _leaderboardData.Entries.RemoveAt(_maxEntries);

        SaveLeaderboard();
        FillEmptyEntries();

        OnEntriesChanged?.Invoke(_leaderboardData);

        return true;
    }

    /// <summary>
    /// Preenche com registros vazios se não houver jogadores suficientes
    /// </summary>
    private void FillEmptyEntries()
    {
        while (_leaderboardData.Entries.Count < _maxEntries)
        {
            _leaderboardData.Entries.Add(new LeaderboardEntry(
                _leaderboardData.Entries.Count + 1, 0, "-----"));
        }
    }

    /// <summary>
    /// Salva leaderboard em arquivo
    /// </summary>
    public void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(_leaderboardData, true);
        File.WriteAllText(_savePath, json);
    }

    /// <summary>
    /// Carrega leaderboard de arquivo
    /// </summary>
    public void LoadLeaderboard()
    {
        if (File.Exists(_savePath))
        {
            string json = File.ReadAllText(_savePath);
            _leaderboardData = JsonUtility.FromJson<LeaderboardData>(json);
        }
        else
        {
            _leaderboardData = new LeaderboardData();
        }
    }

    /// <summary>
    /// Limpa o leaderboard (reset)
    /// </summary>
    public void ClearLeaderboard()
    {
        _leaderboardData = new LeaderboardData();
        FillEmptyEntries();
        SaveLeaderboard();

        OnEntriesChanged?.Invoke(_leaderboardData);
    }
}

[Serializable]
public class LeaderboardEntry
{
    public int Rank;
    public int Score;
    public string PlayerName;

    public LeaderboardEntry(int rank, int score, string playerName)
    {
        Rank = rank;
        Score = score;
        PlayerName = playerName;
    }
}

[Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> Entries = new List<LeaderboardEntry>();
}