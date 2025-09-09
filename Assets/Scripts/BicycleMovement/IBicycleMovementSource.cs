using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public interface IBicycleMovementSource
{
    float GetHandlebarRotation();
    Vector2 GetFrontWheelDirection();
    float GetSpeed();
}
