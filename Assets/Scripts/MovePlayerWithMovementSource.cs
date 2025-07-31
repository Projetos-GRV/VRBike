using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class MovePlayerWithMovementSource : MonoBehaviour
{
    [Tooltip("Eh daqui que virao as informacoes que ditam como e para onde a bicicleta deve se movimentar.")]
    public GameObject movementSourceObject;
    [Tooltip("Habilite somente se o objeto onde este script esta inserido seja o modelo da bicicleta azul (na pasta Sir_bike em assets)")]
    public bool animate = false;
    
    private IBicycleMovementSource movementSource;
    private GameObject handlebar;
    private GameObject backWheel;
    private GameObject frontWheel;
    private Rigidbody rb;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
        IBicycleMovementSource moveSource = this.movementSourceObject.GetComponent<IBicycleMovementSource>();
        if (moveSource == null)
        {
            Debug.LogError("The assigned GameObject does not have a component which implements the IBicycleMovementSource interface.");
            return;
        }
        this.movementSource = moveSource;
        if (this.animate)
        {
            // mais facil de rotacionar o guidao e as rodas
            this.handlebar = this.transform.Find("HandlebarPivot").gameObject;
            this.backWheel = this.transform.Find("Wheel b").gameObject;
            this.frontWheel = this.transform.Find("Wheel f").gameObject;
        }
    }

    void Update()
    {
        if (this.movementSource == null)
        {
            return;
        }
    }

    // RigidBody
    void FixedUpdate()
    {
        if (this.movementSource == null)
        {
            return;
        }
        // velocidade retornada parece ser equivalente a ela multiplicada por aproximadamente 5,8
        // quando aplicada no rigid body usando as medidas padrao das unidades da Unity (1 unidade = 1 metro).
        // Verificacao totalmente empirica. A constante provavelmente devera ser modificada no futuro para coincidir
        // com a escala da cidade.
        float speed = this.movementSource.GetSpeed();

        //Quaternion rotationToDir = Quaternion.LookRotation(Time.fixedDeltaTime * rotated, Vector3.up);
        //rb.rotation = rotationToDir;

        Quaternion deltaRotation = Quaternion.Euler(
            Time.fixedDeltaTime * speed * new Vector3(
                0,
                this.movementSource.GetHandlebarRotation(),
                0
            )
        );
        rb.MoveRotation(rb.rotation * deltaRotation);

        Vector3 forward = transform.forward;
        Vector3 rotated = Quaternion.AngleAxis(this.movementSource.GetHandlebarRotation(), Vector3.up) * forward;
        rb.MovePosition(transform.position + speed * Time.fixedDeltaTime * rotated);

        //rb.AddForce(speed * transform.forward, ForceMode.VelocityChange);
        //rb.velocity = Vector3.ClampMagnitude(rb.velocity, Mathf.Abs(speed));
        if (Debug.isDebugBuild)
        {
            Vector2 dir = this.movementSource.GetFrontWheelDirection();
            Debug.DrawLine(Vector3.zero, new Vector3(dir.x, 0, dir.y) * 10, Color.black, 15);
            Debug.DrawLine(Vector3.zero + new Vector3(0, 2, 0), this.transform.forward.normalized * 10 + new Vector3(0, 2, 0), Color.red, 15);
        }
    }

    //usada para nada exceto verificar a velocidade do objeto em relacao a uma escala
    //private float timer = 1;
    //void LateUpdate()
    //{
    //    timer -= Time.deltaTime;
    //    if (timer <= 0)
    //    {
    //        timer = 1;
    //        Debug.Log(this.transform.position.z);
    //    }
    //}
}
