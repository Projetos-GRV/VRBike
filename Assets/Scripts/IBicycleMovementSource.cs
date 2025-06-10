using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBicycleMovementSource
{
    float GetHandlebarRotationDelta();
    float GetSpeed();
    void Update();
}
