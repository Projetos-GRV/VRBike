using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem; // provavelmente sera desnecessario.... ja que o movimento não virá de um controle.......
using UnityEngine;

[Serializable]
public class BicycleExternalMovementSource : MonoBehaviour, IBicycleMovementSource
{
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

    // provavelmente sera aqui onde os movimentos da bicicleta serao buscados e atualizados
    void Update()
    {
        // buscar os dados da fonte, qualquer que seja essa fonte.
    }

    // essa funcao pode ser mais necessaria do que se pensava se os botoes de arcade forem utilizados
    void OnMove(InputValue movementValue)
    {
        Debug.Log(movementValue.Get<Vector2>());
    }
}
