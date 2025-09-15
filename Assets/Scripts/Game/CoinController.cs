using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinController : MonoBehaviour, ICollectable
{
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private int _value = 10;


    public UnityEvent<CoinController> OnCollected;

    public void HandleObjectCollected()
    {
        IsCollected = true;
        OnCollected?.Invoke(this);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }

    public int Value => _value;
    public bool IsCollected { get; private set; } = false;
}
