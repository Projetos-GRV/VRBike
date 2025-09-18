using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBikeMovement : MonoBehaviour
{
    [Header("Network")]
    [SerializeField] private CustomBikeMovimentSource _dataSource;

    [Header("Player")]
    [SerializeField] private Transform _playerParent;
    [SerializeField] private CancelParentRotation _playerRotationHelper;

    [Header("Bike")]
    [SerializeField] private Rigidbody _bikeRigidbody;
    [SerializeField] private Transform _bikeParent;
    [SerializeField] private Transform _handle;
    [SerializeField] private Transform _frontWheel;
    [SerializeField] private Transform _backWheel;
    [SerializeField] private Transform _pedal;

    [Header("Bike parameters")]
    [SerializeField] private float _turnSpeed = 50f;        // velocidade de rotação da bicicleta
    [SerializeField] private float _handleInfluence = 1f;   // quão forte o guidão influencia
    [SerializeField] private float _wheelRadius = 0.3f;    // raio da roda (m)
    [SerializeField] private float _pedalMultiplier = 2f;   // pedais giram mais rápido que as rodas
    [SerializeField] private float _minHandleDiff = 2f;
    [SerializeField] private float _rotationSpeed = 5f;

    private float _offsetYaw = 0f; // rotação acumulada da bicicleta
    private float _currentYaw = 0f; // rotação acumulada da bicicleta

    [SerializeField] private float _speed;
    [SerializeField] private float _handleAngle;

    private float _prevSpeed = 0f;
    private float _prevHandleAngle = 0f;
    private bool _isMoving = false;

    private float _speedMultiplier = 1f;

    private void Update()
    {
        _speed = _dataSource.Speed * _speedMultiplier;
        _handleAngle = _dataSource.HandleAngle;
    }
    
    private void FixedUpdate()
    {
        if (_prevSpeed != _speed)
        {
            if (_speed <= 0.01f)
            {
                _playerParent.parent = null;
                _isMoving = false;
            }
            else if (!_isMoving)
            {
                _offsetYaw = _bikeParent.localEulerAngles.y;

                _playerRotationHelper.SetReference(_offsetYaw);

                _currentYaw = 0;
                _playerParent.parent = _bikeParent;

                _isMoving = true;
            }

            _prevSpeed = _speed <= 0.01f ? 0f : _speed;
        }

        if (_prevHandleAngle != _handleAngle)
        {
            var newLocalRotation = _handle.localEulerAngles;
            newLocalRotation.y = _handleAngle;

            _handle.localEulerAngles = newLocalRotation;

            _prevHandleAngle = _handleAngle;
        }

        if (_isMoving)
        {
            float handleRotation = _handle.localEulerAngles.y;
            if (handleRotation > 180f) handleRotation -= 360f;

            // acumula rotação mundial (yaw)
            if (Mathf.Abs(handleRotation) > 0.1f)
            {
                _currentYaw += handleRotation * _handleInfluence * Time.deltaTime;
            }

            // aplica rotação no mundo
            Quaternion worldRot = Quaternion.Euler(0, _currentYaw + _offsetYaw, 0);
            _bikeParent.localRotation = worldRot;

            // move para frente no mundo
            _bikeParent.position += _bikeParent.forward * _speed * Time.deltaTime;

            _playerRotationHelper.UpdateRotaion(_currentYaw);

            AnimateWheelsAndPedal();
        }
    }

    void AnimateWheelsAndPedal()
    {
        // Circunferência da roda
        float wheelCircumference = 2 * Mathf.PI * _wheelRadius;

        // Distância percorrida por frame
        float distancePerFrame = _speed * Time.deltaTime;

        // Ângulo de rotação em graus para rodas
        float wheelRotationDegrees = (distancePerFrame / wheelCircumference) * 360f;

        // Roda dianteira
        if (_frontWheel != null)
            _frontWheel.Rotate(Vector3.right, wheelRotationDegrees, Space.Self);

        // Roda traseira
        if (_backWheel != null)
            _backWheel.Rotate(Vector3.right, wheelRotationDegrees, Space.Self);

        // Pedal (mais rápido)
        if (_pedal != null)
            _pedal.Rotate(Vector3.right, wheelRotationDegrees * _pedalMultiplier, Space.Self);
    }

    public void AddMultiplier(float speedMultiplier)
    {
        _speedMultiplier += speedMultiplier;

        _dataSource.HandleSpeedMultplierChanged(_speedMultiplier.ToString("0.000"));
    }

    public void ResetSpeedMultiplier()
    {
        _speedMultiplier = 1f;

        _dataSource.HandleSpeedMultplierChanged(_speedMultiplier.ToString("0.000"));
    }

    public float Speed { get => _speed; set => _speed = value; }

    public float HandleAngle { get => _handleAngle; set => _handleAngle = value; }
    public bool IsMoving => _isMoving;
}
