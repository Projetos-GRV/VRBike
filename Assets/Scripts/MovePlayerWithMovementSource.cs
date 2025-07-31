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
    
    private IBicycleMovementSource movementSource;
    private GameObject handlebar;
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
        //this.handlebar = this.transform.Find("HandlebarPivot").gameObject;
    }

    void Update()
    {

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
