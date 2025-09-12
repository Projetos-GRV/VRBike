using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeTriggerController : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private AudioClip _themeClip;
    [SerializeField] private float _volume = 1f;
    [SerializeField] private bool _inLoop = true;

    public void PlayTheme()
    {
        _audioManager.PlayTheme(_themeClip, _volume, _inLoop);
    }
}
