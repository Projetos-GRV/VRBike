using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public float _minHandleValue;
    public float _maxHandleValue;
    public float _handleThreshold;

    public float _velocityMultiplier;

    public GameData(float minHandleValue, float maxHandleValue, float handleThreshold, float velocityMultiplier)
    {
        _minHandleValue = minHandleValue;
        _maxHandleValue = maxHandleValue;
        _handleThreshold = handleThreshold;

        _velocityMultiplier = velocityMultiplier;
    }

    public GameData()
    {
        _minHandleValue = 0f;
        _maxHandleValue = 0f;
        _handleThreshold = 0f;
        _velocityMultiplier = 1f;
    }
}
