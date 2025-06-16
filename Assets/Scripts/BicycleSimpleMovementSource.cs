using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem; // fica mais facil de atualizar o movimento do jogador sem atrelar demais o código à classe MovePlayerWithMovementSource
using UnityEngine;

[Serializable]
public class BicycleSimpleMovementSource : MonoBehaviour, IBicycleMovementSource
{
    public bool enable = true;

    private bool isMoving { get; set; }
    private float speed { get; set;  }
    private float handlebarRotation { get; set;  }
    private Vector3 defaultDir = Vector3.forward;

    // interface
    public float GetHandlebarRotation() { return this.handlebarRotation; }
    public float GetSpeed() { return this.speed; }
    public void SetForwardDirection(Vector3 dir)
    {
        this.defaultDir.x = dir.x;
        this.defaultDir.y = dir.y;
        this.defaultDir.z = dir.z;
    }

    // monobehaviour
    void Update() {
    
    }

    void Start()
    {
        this.speed = 0;
        this.handlebarRotation = 0;
    }

    // FIX - estou usando o movementVector2D do jeito errado...
    void OnMove(InputValue movementValue)
    {
        if (!this.enable)
        {
            return;
        }

        // calcular angulo do guidao e definir velocidade a partir daqui. 
        Vector2 movementVector2D = movementValue.Get<Vector2>(); // ja esta normalizado
        Debug.Log(movementVector2D);
        if (movementVector2D.y == 0)
        {
            this.speed = 0;
        } else
        {
            this.speed = 1 * (movementVector2D.y > 0 ? 1 : -1); // velocidade constante, pois teclado.
        }

        Vector3 movementVector = new Vector3(movementVector2D.x, 0, movementVector2D.y);
        float res = Vector3.SignedAngle(this.defaultDir, movementVector, Vector3.up);
        res = Mathf.Clamp(res, -80.0f, 80.0f);

        this.handlebarRotation = res;
    }
}
