using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem; // provavelmente sera desnecessario.... ja que o movimento não virá de um controle.......
using UnityEngine;

[Serializable]
public class BicycleExternalMovementSource : MonoBehaviour, IBicycleMovementSource
{
    public bool enable = true;

    public float GetHandlebarRotation() { return 0; }
    public float GetSpeed() { return 0; }
    public void Update() { }
    public void SetForwardDirection(Vector3 dir)
    {
        // definir vetor direcao (para frente)
    }

    void OnMove(InputValue movementValue)
    {
        if (!this.enable)
        {
            return;
        }
    }
}
