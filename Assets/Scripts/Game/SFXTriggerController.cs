using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXTriggerController : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _volume = 1f;

    public void PlaySFX()
    {
        _audioManager.PlaySFX(_audioClip, _volume); 
    }
}
