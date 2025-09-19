using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollctablController : SimpleCollectableController
{
    [SerializeField] private List<AudioClip> _hornsSFXs;
    [SerializeField] private SFXTriggerController _sfxTriggerController;

    private void Awake()
    {
        OnCollected.AddListener(HandleCollected);
    }

    private void HandleCollected(GameObject instance)
    {
        var audioClip = _hornsSFXs.GetRandomItem();

        _sfxTriggerController.PlaySFX(audioClip);
    }
}
