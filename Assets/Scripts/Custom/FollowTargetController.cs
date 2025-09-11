using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _element;

    [Header("Parameters")]
    [SerializeField] private bool _smoothFollowing = true;
    [SerializeField] private float _distance = 1.5f;
    [SerializeField] private float _followSpeed = 1f;

    private void Update()
    {
        if (_target == null || _element == null) return;

        var newPosition = _target.position + _target.forward * _distance;
        var newRotation = Quaternion.LookRotation(_element.position - _target.position, Vector3.up);
        

        if (_smoothFollowing)
        {
            _element.position = Vector3.Lerp(_element.position, newPosition, Time.deltaTime * _followSpeed);
            _element.rotation = Quaternion.Lerp(_element.rotation, newRotation, Time.deltaTime * _followSpeed);
        }
        else
        {
            _element.position = newPosition;
            _element.rotation = newRotation;
        }
    }
}
