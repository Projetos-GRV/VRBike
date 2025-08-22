using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // fica mais facil de atualizar o movimento do jogador sem atrelar demais o c�digo � classe MovePlayerWithMovementSource

[Serializable]
public class BicycleBetterMovementSource : MonoBehaviour, IBicycleMovementSource
{
    public float maxspeed = 3.0f;
    public float accel = 0.5f;
    public float handlebarRotationIncrement = 1f;

    private bool isBraking;
    private bool isMoving;
    private bool isTurning;
    private Vector2 inputVec;

    private float speed;
    private float handlebarRotation;
    private Vector2 direction;

    // interface
    public float GetHandlebarRotation() { return this.handlebarRotation; }
    public Vector2 GetFrontWheelDirection() { return this.direction; }
    public float GetSpeed() { return this.speed; }

    void Update() {
        HandleAccel();
        if (this.isTurning)
        {
            SteerBike(this.inputVec);
        }
    }

    void Start()
    {
        this.speed = 0;
        this.handlebarRotation = 0;
        this.direction = Vector2.up;
        this.isBraking = false;
        this.isMoving = false;
        this.isTurning = false;
        this.inputVec = Vector2.zero;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector2D = movementValue.Get<Vector2>();
        // nao lembro por que eh necessario arredondar as componentes do vetor. favor manter assim
        movementVector2D.x = Mathf.Round(movementVector2D.x);
        movementVector2D.y = Mathf.Round(movementVector2D.y);
        if (movementVector2D.y == 0) // parado
        {
            this.isMoving = false;
            this.isBraking = false;
        }
        else if (movementVector2D.y > 0) // para frente
        {
            this.isMoving = true;
            this.isBraking = false;
        }
        else if (movementVector2D.y < 0) // para tras
        {
            this.isMoving = false;
            this.isBraking = true;
        }

        this.isTurning = movementVector2D.x != 0; // esta curvando!?!?
        this.inputVec = movementVector2D;
    }

    private void HandleAccel()
    {
        float minspeed = 0;
        float acc = 0;

        if (isMoving)
        {
            acc = this.accel;
        }
        else
        {
            acc = -this.accel;
            if (this.isBraking)
            {
                acc = -15.0f;
            }
        }

        this.speed += (acc * Time.deltaTime);
        this.speed = Mathf.Clamp(this.speed, minspeed, this.maxspeed);
    }

    private void SteerBike(Vector2 movementVector2D)  
    {
        float res = this.handlebarRotation;
        if (movementVector2D.x > 0) // direita
        {
            res += (this.handlebarRotationIncrement * Time.deltaTime);
        }
        else if (movementVector2D.x < 0) // esquerda
        {
            res -= (this.handlebarRotationIncrement * Time.deltaTime);
        }
        res = Mathf.Clamp(res, -80.0f, 80.0f);
        Vector3 tmp = Vector3.forward;
        tmp = Quaternion.AngleAxis(res, Vector3.up) * tmp;
        this.direction.x = tmp.x;
        this.direction.y = tmp.z;

        this.handlebarRotation = res;
    }
}
