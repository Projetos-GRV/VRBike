using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettingsReceiver
{
    public List<string> EventsToListen();
    public void HandleEvent(string eventID, string argType, string argValue);
}
