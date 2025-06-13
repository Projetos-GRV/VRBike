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
    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        if (this.movementSource == null)
        {
            return;
        }
        // mover bicicleta a partir das informa��es em moveSource aqui
        // virar camera junto da bicicleta em um script separado para a pr�pria c�mera, mas tamb�m lembrar de permitir o movimento livre desta

        float speed = this.movementSource.GetSpeed();
        float rotation = this.movementSource.GetHandlebarRotation(); 

        GameObject handlebar = this.transform.GetChild(7).gameObject;

        // handlebar.transform.Rotate();

        // atualizar dire��o para frente ao final
        this.movementSource.SetForwardDirection(this.transform.forward);
    }
}
