using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleExternalMovementSource : IBicycleMovementSource
{
    public float GetHandlebarRotationDelta() { return 0; }
    public float GetSpeed() { return 0; }
    public void Update() { }
}
