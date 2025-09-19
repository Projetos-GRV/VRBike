using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeSpeedEffectController : MonoBehaviour
{
    [SerializeField] private CustomBikeMovimentSource _bikeMovementSource;

    [SerializeField] private ParticleSystem _speedLineParticles;

    [SerializeField] private float _maxEmission = 200;
    [SerializeField] private float _minEmisson = 100;
    [SerializeField] private float _maxSpeed = 20;
    [SerializeField] private float _minSpeed = 5;

    private void Awake()
    {
        _bikeMovementSource.OnSpeedChanged.AddListener(HandleSpeedChanged);
    }

    private void HandleSpeedChanged(float newSpeed)
    {
        var emission = _speedLineParticles.emission;

        if (newSpeed < _minSpeed)
        {
            emission.rateOverTime = 0f;
        }
        else
        {
            var formattedSpeed = Mathf.Clamp(newSpeed, _minSpeed, _maxSpeed);
            var percSpeed = (formattedSpeed - _minSpeed) / (_maxSpeed - _minSpeed);

            emission.rateOverTime = ((_maxEmission - _minEmisson) * percSpeed) + _minEmisson;
        }
    }
}
