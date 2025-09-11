using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> _sfxAudioSources;

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
}
