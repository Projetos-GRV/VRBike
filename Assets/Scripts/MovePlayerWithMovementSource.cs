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

    public Transform handlebar;
    public Transform backWheel;
    public Transform frontWheel;
    public Transform pedals;
    [Tooltip("Habilite somente se o objeto onde este script esta inserido seja o modelo da bicicleta azul (na pasta Sir_bike em assets)")]
    public bool animate = false;
    
    private IBicycleMovementSource movementSource;
    private Rigidbody rb;
    private Quaternion handlebarDefaultRotation;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
        if (this.rb == null)
        {
            Debug.LogError("This GameObject does not have a Rigidbody component.");
            return;
        }
        IBicycleMovementSource moveSource = this.movementSourceObject.GetComponent<IBicycleMovementSource>();
        if (moveSource == null)
        {
            Debug.LogError("The assigned GameObject does not have a component which implements the IBicycleMovementSource interface.");
            return;
        }
        this.movementSource = moveSource;
        if (this.handlebar != null)
        {
            StartCoroutine(RotateHandlebar());
        }
        // enquanto uma solucao melhor/mais simples nao for encontrada...
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

    private float currRot = Mathf.Infinity;
    private bool turn = false;
    private void Animate()
    {
        float speed = this.movementSource.GetSpeed();
        float angle = this.movementSource.GetHandlebarRotation();
        float wheelRadius = 0.5f; // um chute.

        float rotSpeed = speed / (2.0f * Mathf.PI * wheelRadius); // rotacoes por segundo
        Debug.Log(turn);
        if (this.handlebar != null)
        {
            turn = (angle != currRot);
            if (turn)
            {
                currRot = angle;
            }
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
            // 0,1943f, 0.02437f ~ valores aproximados do diametro da engrenagenzinha dos pedais (medição totalmente empirica)
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

    IEnumerator RotateHandlebar()
    {
        while (true)
        {
            Debug.Log("Rotating...");
            this.handlebar.RotateAround(this.handlebar.position, this.handlebar.up, this.movementSource.GetHandlebarRotation());
            yield return new WaitUntil(() => turn);
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
