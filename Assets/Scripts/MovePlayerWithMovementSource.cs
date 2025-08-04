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

    public Transform Handlebar;
    public Transform BackWheel;
    public Transform FrontWheel;
    [Tooltip("Habilite somente se o objeto onde este script esta inserido seja o modelo da bicicleta azul (na pasta Sir_bike em assets)")]
    public bool animate = false;
    
    private IBicycleMovementSource movementSource;
    private Rigidbody rb;
    private Quaternion handlebarDefaultRotation;

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
        if (this.Handlebar != null)
        {
            this.handlebarDefaultRotation = this.Handlebar.rotation;
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

        // ok
        Vector3 forward = transform.forward;
        Vector3 rotated = Quaternion.AngleAxis(this.movementSource.GetHandlebarRotation(), Vector3.up) * forward;
        rb.MovePosition(transform.position + speed * Time.fixedDeltaTime * rotated);

        //rb.AddForce(speed * transform.forward, ForceMode.VelocityChange);
        //rb.velocity = Vector3.ClampMagnitude(rb.velocity, Mathf.Abs(speed));
        if (Debug.isDebugBuild)
        {
            Vector2 dir = this.movementSource.GetFrontWheelDirection();
            Debug.DrawLine(Vector3.zero, new Vector3(dir.x, 0, dir.y) * 10, Color.black, 15);
            Debug.DrawLine(Vector3.zero + new Vector3(0, 1, 0), rotated * 10 + new Vector3(0, 1, 0), Color.red, 15);
            //float angle = Vector3.SignedAngle(rotated, Vector3.forward, Vector3.up);
            //float handleAngle = this.movementSource.GetHandlebarRotation();
            //Debug.Log("MSource angle: " + handleAngle + "\nThis angle: " + angle);
        }
    }

    void LateUpdate()
    {
        if (this.animate)
        {
            Animate();
        }
    }

    private bool turn;
    private float currRot = Mathf.Infinity;
    private void Animate()
    {
        float speed = this.movementSource.GetSpeed();
        float angle = this.movementSource.GetHandlebarRotation();
        float wheelRadius = 0.5f; // um chute.

        float rotSpeed = speed / (2.0f * Mathf.PI * wheelRadius); // rotacoes por segundo
        turn = (angle != currRot);
        if (this.Handlebar)
        {
            // TODO - achar um jeito de virar o guidao somente uma vez caso o angulo nao tenha mudado
            if (turn)
            {
                currRot = angle;
                this.Handlebar.RotateAround(this.Handlebar.position, this.Handlebar.up, angle);
            }
        }
        if (this.FrontWheel)
        {
            this.FrontWheel.RotateAround(this.FrontWheel.position, this.FrontWheel.right, 360 * rotSpeed * Time.deltaTime);
        }
        if (this.BackWheel)
        {
            this.BackWheel.RotateAround(this.BackWheel.position, this.BackWheel.right, 360 * rotSpeed * Time.deltaTime);
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
