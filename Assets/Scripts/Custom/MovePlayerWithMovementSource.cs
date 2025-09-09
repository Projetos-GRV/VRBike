using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class MovePlayerWithMovementSource : MonoBehaviour
{
    [Tooltip("O primeiro controlador filho eh escolhido como o objeto controlador. Eh daqui que serao retirados ambos angulo do guidao e velocidade usados para movimentar a bicicleta. Todos os GameObjects filhos devem conter um script que implemente a interface IBicycleMovementSource.")]
    public GameObject bicycleControllersObject;

    [Tooltip("Opcional. Pode ser nulo.")]
    public Transform handlebar;
    [Tooltip("Opcional. Pode ser nulo.")]
    public Transform backWheel;
    [Tooltip("Opcional. Pode ser nulo.")]
    public Transform frontWheel;
    [Tooltip("Opcional. Pode ser nulo.")]
    public Transform pedals;
    [Tooltip("Caso habilitado, havera uma tentativa de animar os Transforms correspondentes a diferentes partes da bicicleta (ex.: as rodas). Sugere-se habilitar animacoes somente quando for utilizado o modelo de bicicleta azul (na pasta Sir_bike em assets)")]
    public bool animate = false;
    
    private IBicycleMovementSource movementSource;
    private Rigidbody rb;
    private Vector3 handlebarDefaultRotation;

    private bool _isEnabled = false;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
        if (this.rb == null)
        {
            Debug.LogError("This GameObject does not have a Rigidbody component.");
            return;
        }
        if (this.bicycleControllersObject != null)
        {
            GameObject movementSourceObject = null;
            foreach (Transform child in this.bicycleControllersObject.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    movementSourceObject = child.gameObject;
                    break;
                }
            }
            IBicycleMovementSource moveSource = movementSourceObject.GetComponent<IBicycleMovementSource>();
            if (moveSource == null)
            {
                Debug.LogError("The assigned GameObject does not have a component which implements the IBicycleMovementSource interface.");
                return;
            }
            this.movementSource = moveSource;
        }
        if (this.handlebar != null)
        {
            this.handlebarDefaultRotation = this.handlebar.transform.eulerAngles;
        }
    }

    // RigidBody
    void FixedUpdate()
    {
        /*
        if (this.movementSource == null || !_isEnabled)
        {
            return;
        }

        float speed = this.movementSource.GetSpeed();
        float rotation = this.movementSource.GetHandlebarRotation();
        //Quaternion rotationToDir = Quaternion.LookRotation(Time.fixedDeltaTime * rotated, Vector3.up);
        //rb.rotation = rotationToDir;

        Quaternion deltaRotation = Quaternion.Euler(
            Time.fixedDeltaTime * speed * new Vector3(0, rotation, 0)
        );
        rb.MoveRotation(rb.rotation * deltaRotation);

        // ok
        Vector3 forward = transform.forward;
        Vector3 rotated = Quaternion.AngleAxis(rotation, Vector3.up) * forward;
        rb.MovePosition(transform.position + speed * Time.fixedDeltaTime * rotated);

        // so para confirmar se o vetor de direcao do guidao no movementSource esta correto
        if (Debug.isDebugBuild)
        {
            Vector2 dir = this.movementSource.GetFrontWheelDirection();
            Debug.DrawLine(Vector3.zero, new Vector3(dir.x, 0, dir.y) * 10, Color.black, 15);
            Debug.DrawLine(Vector3.zero + new Vector3(0, 1, 0), rotated * 10 + new Vector3(0, 1, 0), Color.red, 15);
            //float angle = Vector3.SignedAngle(rotated, Vector3.forward, Vector3.up);
            //float handleAngle = this.movementSource.GetHandlebarRotation();
            //Debug.Log("MSource angle: " + handleAngle + "\nThis angle: " + angle);
        }
        */
    }

    void LateUpdate()
    {
        if (this.animate && this.movementSource != null && _isEnabled)
        {
            Animate();
        }
    }

    private void Animate()
    {
        float speed = this.movementSource.GetSpeed();
        float angle = this.movementSource.GetHandlebarRotation();
        float wheelRadius = 0.3f; // um chute.

        float rotSpeed = speed / (2.0f * Mathf.PI * wheelRadius); // rotacoes por segundo
        if (this.handlebar != null)
        {
            Quaternion baseRot = Quaternion.Euler(this.handlebarDefaultRotation.x, this.handlebarDefaultRotation.y, this.handlebarDefaultRotation.z);
            Quaternion turnRot = Quaternion.Euler(0.0f, angle, 0.0f);
            this.handlebar.localRotation = baseRot * turnRot;
        }
        if (this.frontWheel != null)
        {
            this.frontWheel.RotateAround(this.frontWheel.position, this.frontWheel.right, 360 * rotSpeed * Time.deltaTime);
        }
        if (this.backWheel != null)
        {
            this.backWheel.RotateAround(this.backWheel.position, this.backWheel.right, 360 * rotSpeed * Time.deltaTime);
        }
        if (this.pedals != null)
        {
            // 0.25f ~ valor aproximado do raio da circunferencia que os pedais formam quando rotacionam (medicao tambem empirica)
            float pedalSpeed = speed / (2.0f * Mathf.PI * 0.25f);
            this.pedals.RotateAround(this.pedals.position, this.pedals.right, 360 * pedalSpeed * Time.deltaTime);
            if (this.pedals.childCount > 0)
            {
                foreach (Transform cube in this.pedals)
                {
                    cube.RotateAround(cube.position, cube.right, -360 * pedalSpeed * Time.deltaTime);
                }
            }
        }
    }

    public void DisableTracking()
    {
        _isEnabled = false;
    }

    public void EnableTracking()
    {
        _isEnabled = true;
    }
}
