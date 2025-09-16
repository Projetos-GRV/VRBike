using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXTriggerController : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _volume = 1f;

    private void Awake()
    {
        if (_audioManager == null && AudioManager.Instance != null)
            _audioManager = AudioManager.Instance;
    }

    public void PlaySFX()
    {
        _audioManager.PlaySFX(_audioClip, _volume); 
    }
}
