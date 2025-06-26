using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class MovePlayerWithMovementSource : MonoBehaviour
{
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
        // se nao houver de onde pegar os movimentos, nn faz nada e segue a vida
        if (this.movementSource == null)
        {
            return;
        }
        // mover bicicleta a partir das informacoes em moveSource aqui
        // virar camera junto da bicicleta em um script separado para a propria camera, mas tamb�m lembrar de permitir o movimento livre desta

        //float speed = this.movementSource.GetSpeed();
        //float rotation = this.movementSource.GetHandlebarRotation();

        // this.handlebar.transform.Rotate();
        //this.transform.Rotate(Time.deltaTime * rotation * Vector3.up);
        //this.transform.Translate(Time.deltaTime * speed * this.transform.forward);

        // atualizar direcao para frente ao final
    }

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
        float speed = this.movementSource.GetSpeed() / 5.8f;
        rb.AddForce(speed* transform.forward, ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, Mathf.Abs(speed));

        Quaternion deltaRotation = Quaternion.Euler(
            Time.fixedDeltaTime * 2 * new Vector3(
                0,
                this.movementSource.GetHandlebarRotation(),
                0
            )
        );
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private float timer = 1;
    void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 1;
            Debug.Log(this.transform.position.z);
        }
    }
}
