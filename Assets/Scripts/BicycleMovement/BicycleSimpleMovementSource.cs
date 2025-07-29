using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // fica mais facil de atualizar o movimento do jogador sem atrelar demais o código à classe MovePlayerWithMovementSource

[Serializable]
public class BicycleSimpleMovementSource : MonoBehaviour, IBicycleMovementSource
{
    public bool enableMovement = true;
    [Tooltip("Ativa/desativa movimentos simples, onde direcao e velocidade sofrem alteracoes instantaneas. A bicicleta tambem podera andar de re caso ativado, embora isso nao seja possivel na vida real.")]
    public bool simplerMovements = true;
    public float maxspeed = 0.7f;
    public float accel = 0.5f;
    public float handlebarRotationIncrement = 1f;

    private bool isMoving { get; set; }
    // Bicicletas nao andam de re, mas enfim...
    private bool goingBackwards { get; set; }
    private float speed { get; set; }
    private float handlebarRotation { get; set; }
    private Vector2 direction { get; set; }

    // interface
    public float GetHandlebarRotation() { return this.handlebarRotation; }
    public Vector2 GetFrontWheelDirection() { return this.direction; } // TODO - retornar um valor diferente? Atualizar conforme rotacao
    public float GetSpeed() { return this.speed; }

    // monobehaviour
    void Update() {
        if (!this.enableMovement)
        {
            return;
        }
        HandleAccel();
    }

    void Start()
    {
        this.speed = 0;
        this.accel = 0.5f;
        this.handlebarRotation = 0;
        this.goingBackwards = false;
        this.direction = Vector3.zero;
    }

    void OnMove(InputValue movementValue)
    {
        if (!this.enableMovement)
        {
            return;
        }

        // calcular angulo do guidao e definir velocidade a partir daqui.
        Vector2 movementVector2D = movementValue.Get<Vector2>();
        if (simplerMovements)
        {
            SimpleVectorBasedMovement(movementVector2D);
        } else
        {
            AngleIncrementBasedMovement(movementVector2D);
        }
    }

    private void AngleIncrementBasedMovement(Vector2 movementVector2D)
    {
        Debug.Log("Moving!!");

        if (Debug.isDebugBuild)
        {
            //Debug.Log("Vector: " + movementVector2D);
            //Debug.Log("Rotation: " + this.handlebarRotation);
            //Debug.Log("Speed: " + this.speed);
        }
        this.goingBackwards = false;
        if (movementVector2D.y <= 0)
        {
            this.isMoving = false;
        } else if (movementVector2D.y > 0)
        {
            this.isMoving = true;
            float res = this.handlebarRotation;
            if (movementVector2D.x > 0)
            {
                res -= this.handlebarRotationIncrement;
                //res = Mathf.Clamp(res, 80.0f, -80.0f);
            }
            else if (movementVector2D.x < 0)
            {
                res += this.handlebarRotationIncrement;
                //res = Mathf.Clamp(res, 80.0f, -80.0f);
            }
            this.handlebarRotation = res;
        }
    }

    private void SimpleVectorBasedMovement(Vector2 movementVector2D)
    {
        if (Debug.isDebugBuild)
        {
            Debug.Log(movementVector2D.normalized);
        }
        //this.direction = movementVector2D.normalized;
        if (movementVector2D.y == 0)
        {
            this.isMoving = false;
            this.goingBackwards = false;
            this.speed = 0;
        }
        else if (movementVector2D.y > 0)
        {
            this.isMoving = true;
            this.goingBackwards = false;
            this.speed = this.maxspeed;
        }
        else if (movementVector2D.y == -1)
        {
            this.isMoving = true;
            this.goingBackwards = true;
            this.speed = -this.maxspeed;
        }

        movementVector2D.Normalize();
        Vector3 movementVector = new Vector3(movementVector2D.x, 0, movementVector2D.y);
        Vector3 forward = Vector3.forward;
        float res = Vector3.SignedAngle(forward, movementVector, Vector3.up);
        if (Mathf.Abs(res) > 90)
        {
            res = 0;
        }
        res = Mathf.Clamp(res, -80.0f, 80.0f);
        Vector3 tmp = this.direction;
        tmp = Quaternion.AngleAxis(res, Vector3.up) * tmp;
        this.direction = tmp;
        if (Debug.isDebugBuild)
        {
            Debug.Log(this.direction);
        }

        this.handlebarRotation = res;
    }

    // Pode nao executar se {simplerMovements} for true;
    private void HandleAccel()
    {
        if (this.simplerMovements)
        {
            return;
        }
        float minspeed = 0;
        float lMaxspeed = this.maxspeed;
        float acc = 0;

        if (isMoving)
        {
            acc = this.accel;
            if (goingBackwards)
            {
                minspeed = -lMaxspeed;
                acc = -this.accel;
            }
        }
        else
        {
            if (this.speed < 0)
            {
                minspeed = -lMaxspeed;
                lMaxspeed = 0;
                acc = this.accel;
            }
            else if (this.speed > 0)
            {
                acc = -this.accel;
            }
        }

        this.speed += (acc * Time.deltaTime);
        this.speed = Mathf.Clamp(this.speed, minspeed, lMaxspeed);
    }
}
