using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollisionDetectorController : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent<GameObject> OnPlayerCollideWithObject;

    private void OnTriggerEnter(Collider other)
    {
        OnPlayerCollideWithObject?.Invoke(other.gameObject);
    }
}
