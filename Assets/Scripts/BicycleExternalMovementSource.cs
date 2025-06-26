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

    void Update() { } // provavelmente sera aqui onde os movimentos da bicicleta serao buscados e atualizados

    void OnMove(InputValue movementValue) // pode ser que nao seja necessario, ja que os movimentos virao de fora
    {
        if (!this.enable)
        {
            return;
        }
    }
}
