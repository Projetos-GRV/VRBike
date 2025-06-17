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

    void Start()
    {
        IBicycleMovementSource moveSource = this.movementSourceObject.GetComponent<IBicycleMovementSource>();
        if (moveSource == null)
        {
            Debug.LogError("The assigned GameObject does not have a component which implements the interface IBicycleMovementSource.");
            return;
        }
        // definir vetor para frente logo no come�o
        moveSource.SetForwardDirection(this.transform.forward);
        this.movementSource = moveSource;
        this.handlebar = this.transform.Find("HandlebarPivot").gameObject;
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

        float speed = this.movementSource.GetSpeed();
        float rotation = this.movementSource.GetHandlebarRotation();

        // this.handlebar.transform.Rotate();
        //this.transform.Rotate(Time.deltaTime * rotation * Vector3.up);
        this.transform.Translate(Time.deltaTime * speed * this.transform.forward);

        // atualizar direcao para frente ao final
        this.movementSource.SetForwardDirection(this.transform.forward);
    }
}
