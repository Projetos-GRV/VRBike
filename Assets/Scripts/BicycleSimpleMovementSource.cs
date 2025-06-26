using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // fica mais facil de atualizar o movimento do jogador sem atrelar demais o código à classe MovePlayerWithMovementSource

[Serializable]
public class BicycleSimpleMovementSource : MonoBehaviour, IBicycleMovementSource
{
    public bool enable = true;
    public float maxspeed = .7f;
    public float accel = 0.5f;

    private bool isMoving { get; set; }
    private bool goingBackwards { get; set; }
    private float speed { get; set; }
    private float handlebarRotation { get; set; }

    // interface
    public float GetHandlebarRotation() { return this.handlebarRotation; }
    public float GetSpeed() { return this.speed; }

    // monobehaviour
    void Update() {
        //float minspeed = 0;
        //float lMaxspeed = this.maxspeed;
        //float acc = 0;

        //if (isMoving)
        //{
        //    acc = this.accel;
        //    if (goingBackwards)
        //    {
        //        minspeed = -lMaxspeed;
        //        acc = -this.accel;
        //    }
        //} else
        //{
        //    if (this.speed < 0)
        //    {
        //        minspeed = -lMaxspeed;
        //        lMaxspeed = 0;
        //        acc = this.accel;
        //    } else if (this.speed > 0)
        //    {
        //        acc = -this.accel;
        //    }
        //}

        //this.speed += (acc * Time.deltaTime);
        //this.speed = Mathf.Clamp(this.speed, minspeed, lMaxspeed);
    }

    void Start()
    {
        this.speed = 0;
        this.accel = 0.5f;
        this.handlebarRotation = 0;
        this.goingBackwards = false;
    }

    void OnMove(InputValue movementValue)
    {
        if (!this.enable)
        {
            return;
        }

        // calcular angulo do guidao e definir velocidade a partir daqui. 
        Vector2 movementVector2D = movementValue.Get<Vector2>(); // ja esta normalizado
        if (movementVector2D.y == 0)
        {
            this.isMoving = false;
            this.goingBackwards = false;
            this.speed = 0;
        } else if (movementVector2D.y > 0) {
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

        // FIX - movementVector2D nao considera direcao do objeto. Os vetores retornados sao sempre os mesmos pras mesmas sequencias de teclas pressionadas.
        movementVector2D.Normalize();
        Vector3 movementVector = new Vector3(movementVector2D.x, 0, movementVector2D.y);
        Vector3 forward = Vector3.forward;
        float res = Vector3.SignedAngle(forward, movementVector, Vector3.up);
        if (Mathf.Abs(res) > 90)
        {
            res = 0;
        }
        res = Mathf.Clamp(res, -80.0f, 80.0f);

        this.handlebarRotation = res;
    }
}
