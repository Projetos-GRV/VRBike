using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class UIDebbugerController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _view;

    [Header("Configurações")]
    [SerializeField] private int maxMessages = 20; // quantidade máxima antes de limpar
    [SerializeField] private string logFileName = "VR_DebugLog.txt";


    [SerializeField] private TextMeshProUGUI _txtLog;

    private Queue<string> messages = new Queue<string>();

    private void Awake()
    {
        Application.logMessageReceived += Application_logMessageReceived;
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        // adiciona mensagem
        string timeMsg = $"[{System.DateTime.Now:HH:mm:ss}][{type}]{condition}";
        messages.Enqueue(timeMsg);

        // se passou do limite, salva e limpa
        if (messages.Count > maxMessages)
        {
            SaveLogToFile();
            messages.Clear();
        }

        // atualiza TMP
        UpdateText();
    }

    /// <summary>
    /// Atualiza o TextMeshPro com o histórico atual.
    /// </summary>
    private void UpdateText()
    {
        _txtLog.text = string.Join("\n", messages);
    }

    /// <summary>
    /// Salva o histórico atual em um arquivo texto na pasta persistente.
    /// </summary>
    private void SaveLogToFile()
    {
        string path = Path.Combine(Application.persistentDataPath, logFileName);

        try
        {
            File.AppendAllLines(path, messages);
            Debug.Log($"[VRDebugger] Log salvo em: {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[VRDebugger] Erro ao salvar log: {e.Message}");
        }
    }

    private void OnApplicationQuit()
    {
        SaveLogToFile();
    }

    public void ToggleVisibity(bool value)
    {
        _view.SetActive(value);
    }
}
