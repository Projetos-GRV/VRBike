using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem; // fica mais facil de atualizar o movimento do jogador sem atrelar demais o código à classe MovePlayerWithMovementSource
using UnityEngine;

[Serializable]
public class BicycleSimpleMovementSource : MonoBehaviour, IBicycleMovementSource
{
    public bool enable = true;

    private float speed = 0;
    private float handlebarRotation = 0;
    private Vector3 defaultDir = Vector3.forward;

    public float GetHandlebarRotation() { return this.handlebarRotation; }
    public float GetSpeed() { return this.speed; }
    public void Update() { }
    public void SetForwardDirection(Vector3 dir)
    {
        this.defaultDir.x = dir.x;
        this.defaultDir.y = dir.y;
        this.defaultDir.z = dir.z;
    }

    void OnMove(InputValue movementValue)
    {
        if (!this.enable)
        {
            return;
        }

        // calcular angulo do guidao a partir daqui. 
        Vector2 movementVector2D = movementValue.Get<Vector2>(); // ja esta normalizado
        Vector3 movementVector = new Vector3(movementVector2D.x, 0, movementVector2D.y);
        float res = Vector3.SignedAngle(this.defaultDir, movementVector, Vector3.up);

        Debug.Log(res);

        this.handlebarRotation = res;

        // atualizar defaultDir
    }
}
