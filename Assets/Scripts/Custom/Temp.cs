using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Temp : MonoBehaviour
{
    public TextMeshProUGUI _txtLog;

    string fileName = "temp.txt";

    public void SaveData(bool value)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string content = Random.Range(0f, 1f).ToString();

        File.WriteAllText(filePath, content);
        _txtLog.text = $"Save {content}";
    }

    public void LoadData()
    {
        _txtLog.text = "Load ";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        string content = File.ReadAllText(filePath);
        _txtLog.text += content;
    }
}
