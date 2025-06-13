using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public interface IBicycleMovementSource
{
    float GetHandlebarRotation();
    float GetSpeed();
    void SetForwardDirection(Vector3 dir);
}
