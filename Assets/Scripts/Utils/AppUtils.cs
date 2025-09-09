using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public static class AppUtils
{

    public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        // Avoid division by zero in case the original range is zero.
        if (Mathf.Abs(fromMax - fromMin) < float.Epsilon)
        {
            return toMin;
        }

        // Apply the formula.
        return Mathf.Clamp((value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin, toMin, toMax);
    }

    public static void SetPositionInFront(Transform obj, Transform target, float distance)
    {
        Vector3 forward = target.forward;
        forward.y = 0; // zera a componente vertical
        forward.Normalize();

        // Calcula posição em frente ao HMD
        Vector3 targetPos = target.position + forward * distance;

        // Aplica a posição
        obj.position = targetPos;

        // Faz o objeto olhar para o HMD, mas só em Y
        Vector3 lookDirection = target.position - obj.position;
        lookDirection.y = 0; // trava o eixo Y
        if (lookDirection.sqrMagnitude > 0.001f)
            obj.rotation = Quaternion.LookRotation(-lookDirection, Vector3.up);
    }
}
