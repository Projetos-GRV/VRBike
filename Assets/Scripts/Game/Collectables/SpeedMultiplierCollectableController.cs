using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMultiplierCollectableController : SimpleCollectableController
{
    [SerializeField] private float _speedMultiplier = 0.25f;

    public float SpeedMultiplier => _speedMultiplier;
}
