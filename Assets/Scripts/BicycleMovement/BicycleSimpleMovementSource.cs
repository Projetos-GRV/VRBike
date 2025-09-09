using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // fica mais facil de atualizar o movimento do jogador sem atrelar demais o c�digo � classe MovePlayerWithMovementSource

[Serializable]
public class BicycleSimpleMovementSource : MonoBehaviour, IBicycleMovementSource
{
    [Tooltip("Quando ativado, o controle de curva sera instantaneo em vez de ser necessario pressionar a tecla de direcao multiplas vezes.")]
    public bool simplerSteering = false;
    public bool instantSpeed = false;
    public float maxspeed = 3.0f;
    public float accel = 0.5f;
    [Tooltip("Nao sera aplicado se Simpler Steering estiver habilitado.")]
    public float handlebarRotationIncrement = 1f;

    private bool isBraking;
    private bool isMoving;
    private float speed;
    private float handlebarRotation;
    private Vector2 direction;

    // interface
    public float GetHandlebarRotation() { return this.handlebarRotation; }
    public Vector2 GetFrontWheelDirection() { return this.direction; }
    public float GetSpeed() { return this.speed; }

    // monobehaviour
    void Update() {
        HandleAccel();
    }

    void Start()
    {
        this.speed = 0;
        this.handlebarRotation = 0;
        this.direction = Vector2.up;
        this.isBraking = false;
        this.isMoving = false;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector2D = movementValue.Get<Vector2>();
//        movementVector2D.x = Mathf.Round(movementVector2D.x);
 //       movementVector2D.y = Mathf.Round(movementVector2D.y);
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

        if (simplerSteering)
        {
            SimpleVectorBasedSteering(movementVector2D);
        } else
        {
            AngleIncrementBasedSteering(movementVector2D);
        }
    }

    private void HandleAccel()
    {
        if (this.instantSpeed)
        {
            this.speed = this.isMoving ? this.maxspeed : 0;
            return;
        }
        float minspeed = 0;
        float lMaxspeed = this.maxspeed;
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
        this.speed = Mathf.Clamp(this.speed, minspeed, lMaxspeed);
    }

    private void AngleIncrementBasedSteering(Vector2 movementVector2D)
    {
        float res = this.handlebarRotation;
        if (movementVector2D.x > 0) // direita
        {
            res += this.handlebarRotationIncrement;
        }
        else if (movementVector2D.x < 0) // esquerda
        {
            res -= this.handlebarRotationIncrement;
        }
        res = Mathf.Clamp(res, -80.0f, 80.0f);
        Vector3 tmp = Vector3.forward;
        tmp = Quaternion.AngleAxis(res, Vector3.up) * tmp;
        this.direction.x = tmp.x;
        this.direction.y = tmp.z;

        this.handlebarRotation = res;
    }

    private void SimpleVectorBasedSteering(Vector2 movementVector2D)
    {
        //this.direction = movementVector2D.normalized;
        movementVector2D.Normalize();
        Vector3 movementVector = new Vector3(movementVector2D.x, 0, movementVector2D.y);
        Vector3 forward = Vector3.forward;
        float res = Vector3.SignedAngle(forward, movementVector, Vector3.up);
        if (Mathf.Abs(res) > 90)
        {
            res = 0;
        }
        res = Mathf.Clamp(res, -80.0f, 80.0f);
        Vector3 tmp = Vector3.forward;
        tmp = Quaternion.AngleAxis(res, Vector3.up) * tmp;
        //this.direction = tmp;
        this.direction.x = tmp.x;
        this.direction.y = tmp.z;

        this.handlebarRotation = res;
    }
}
