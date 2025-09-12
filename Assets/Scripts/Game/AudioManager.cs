using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> _sfxAudioSources;
    [SerializeField] private AudioSource _themeAudioSource;

    private int _currentSFXAudioSourceIndex;

    private void Awake()
    {
        _currentSFXAudioSourceIndex = 0;
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        if (+_sfxAudioSources.Count == 0) return;

        var audioSource = _sfxAudioSources[_currentSFXAudioSourceIndex];

        audioSource.clip = clip;
        audioSource.volume = volume;

        audioSource.Play();

        _currentSFXAudioSourceIndex = (_currentSFXAudioSourceIndex + 1) % _sfxAudioSources.Count;
    }

    public void PlayTheme(AudioClip themeClip, float volume, bool inLoop)
    {
        _themeAudioSource.clip = themeClip;
        _themeAudioSource.volume = volume;
        _themeAudioSource.loop = inLoop;

        _themeAudioSource.Play();
    }
}
