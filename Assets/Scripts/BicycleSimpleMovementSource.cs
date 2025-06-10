using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleSimpleMovementSource : IBicycleMovementSource
{
    public float speed = 0;
    public float GetHandlebarRotationDelta() { return 0; }
    public float GetSpeed() { return this.speed; }
    public void Update() { }
}
