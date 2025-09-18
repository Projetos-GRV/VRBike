using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeTriggerController : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private AudioClip _themeClip;
    [SerializeField] private float _volume = 1f;
    [SerializeField] private bool _inLoop = true;

    [Header("Paramters")]
    [SerializeField] private bool _playOnTrueBoolEvent = false;
    [SerializeField] private bool _playOnFalseBoolEvent = false;

    public void PlayTheme()
    {
        _audioManager.PlayTheme(_themeClip, _volume, _inLoop);
    }

    public void HandleBoolValue(bool value)
    {
        if (_playOnTrueBoolEvent && value)
        {
            PlayTheme();
        }
        else if (_playOnFalseBoolEvent && !value)
        {
            PlayTheme();
        }
    }
}
