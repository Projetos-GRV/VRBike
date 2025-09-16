using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleCollectableController : MonoBehaviour, ICollectable
{
    [SerializeField] protected bool _destroyOnCollect = false;

    [Header("Events")]
    public UnityEvent<GameObject> OnCollected;

    public void HandleObjectCollected()
    {
        OnCollected?.Invoke(gameObject);

        if (_destroyOnCollect)
        {
            Destroy(gameObject);
        }
    }
}
