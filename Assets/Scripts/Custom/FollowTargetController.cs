using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _target;

    [Header("Rotation Parameters")]
    [SerializeField] private bool _followRotation;
    [SerializeField] private bool _rotationX;
    [SerializeField] private bool _rotationY;
    [SerializeField] private bool _rotationZ;


    private void Update()
    {
        if (_followRotation)
        {
            var eulerAngles = transform.eulerAngles;

            eulerAngles.x = _rotationX ? _target.eulerAngles.x : eulerAngles.x;
            eulerAngles.y = _rotationY ? _target.eulerAngles.y : eulerAngles.y;
            eulerAngles.z = _rotationZ ? _target.eulerAngles.z : eulerAngles.z;

            transform.eulerAngles = eulerAngles;
        }
    }
}
