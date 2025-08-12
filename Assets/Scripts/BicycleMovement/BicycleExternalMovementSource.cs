using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem; // provavelmente sera desnecessario.... ja que o movimento não virá de um controle.......
using UnityEngine;

[Serializable]
public class BicycleExternalMovementSource : MonoBehaviour, IBicycleMovementSource
{
    public bool enable = true;

    private float speed;
    private float handlebarRotation;
    private Vector2 direction;

    public float GetHandlebarRotation() { return this.handlebarRotation; }
    public Vector2 GetFrontWheelDirection() { return this.direction; }
    public float GetSpeed() { return this.speed; }

    void Start()
    {
        this.speed = 0;
        this.handlebarRotation = 0;
        this.direction = Vector2.zero;
    }

    void Update() { } // provavelmente sera aqui onde os movimentos da bicicleta serao buscados e atualizados

    void OnMove(InputValue movementValue) // pode ser que nao seja necessario, ja que os movimentos virao de fora
    {
        if (!this.enable)
        {
            return;
        }
    }
}
