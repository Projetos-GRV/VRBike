using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[Serializable]
public class MovePlayerWithMovementSource : MonoBehaviour
{
    public GameObject movementSource;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        IBicycleMovementSource moveSource = this.movementSource.GetComponent<IBicycleMovementSource>();
        if (moveSource == null)
        {
            Debug.LogError("The assigned GameObject does not have a component which implements the interface IBicycleMovementSource.");   
        }
        moveSource.SetForwardDirection(this.transform.forward);

        // mover bicicleta a partir das informações em moveSource aqui
    }
}
