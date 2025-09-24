using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsServerManager : MonoBehaviour
{
    public string _messageSeparator = "<->";

    public UDPReceiver _udpReceiver;

    public List<GameObject> _settingsReceivers;
    public Dictionary<string, List<ISettingsReceiver>> _listeners;

    private void Start()
    {
        _listeners = new Dictionary<string, List<ISettingsReceiver>>();

        foreach (var receiver in _settingsReceivers)
        {
            var settingReceiver = receiver.GetComponent<ISettingsReceiver>();
            var eventsToListen = settingReceiver.EventsToListen();

            foreach (var e in eventsToListen)
            {
                if (!_listeners.ContainsKey(e)) _listeners.Add(e, new List<ISettingsReceiver>());

                _listeners[e].Add(settingReceiver);
            }
        }
    }

    private void Update()
    {
        if (_udpReceiver.messageReady)
        {
            HandleMessageReceiver(_udpReceiver.lastMessage);
            _udpReceiver.messageReady = false;
        }
    }

    private void HandleMessageReceiver(string message)
    {
        var lines = message.Split('\n');

        foreach (var line in lines)
        {
            var substrings = line.Split(_messageSeparator);

            if (substrings.Length >= 3)
            {
                var eventID = substrings[0];
                var argType = substrings[1];
                var argValue = substrings[2];

                if (_listeners.ContainsKey(eventID))
                {
                    _listeners[eventID].ForEach(listener => listener.HandleEvent(eventID, argType, argValue));

                    Debug.Log($"[HandleMessageReceiver]     {eventID} {argType} {argValue}");
                }
                else
                {
                    Debug.Log($"[HandleMessageReceiver]     {eventID} not found!");
                }
            }
            else
            {
                Debug.Log($"[HandleMessageReceiver] Mensagem mal formatada!");
            }
        }
    }
}
