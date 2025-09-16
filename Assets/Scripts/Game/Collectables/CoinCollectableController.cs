using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinCollectableController: SimpleCollectableController
{
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private int _value = 10;

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }

    public int Value => _value;
}
