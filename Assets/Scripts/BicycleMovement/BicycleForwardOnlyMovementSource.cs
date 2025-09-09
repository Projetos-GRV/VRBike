using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BicycleForwardMovementSource : MonoBehaviour, IBicycleMovementSource
{
    public bool instantSpeed = false;
    public float maxspeed = 3.0f;
    public float accel = 0.5f;

    private bool isMoving;
    private bool isBraking;
    private float speed;

    // interface
    public float GetHandlebarRotation() { return 0; }
    public Vector2 GetFrontWheelDirection() { return Vector2.up; }
    public float GetSpeed() { return this.speed; }

    // monobehaviour
    void Start()
    {
        this.isMoving = false;
        this.isBraking = false;
        this.speed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAccel();
    }

    private void HandleAccel()
    {
        if (this.instantSpeed)
        {
            this.speed = this.isMoving ? this.maxspeed : 0;
            return;
        }
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

    void OnMove(InputValue movementValue)
    {
        // Checar se esta se movendo para frente (e somente para frente)
        Vector2 dir = movementValue.Get<Vector2>();
        if (dir.y == 0)
        {
            this.isMoving = false;
            this.isBraking = false;
        }
        else if (dir.y > 0)
        {
            this.isMoving = true;
            this.isBraking = false;
        } else if (dir.y < 0)
        {
            this.isMoving = false;
            this.isBraking = true;
        }
    }
}
