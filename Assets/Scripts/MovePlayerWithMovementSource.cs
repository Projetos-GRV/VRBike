using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerWithMovementSource : MonoBehaviour
{
    public IBicycleMovementSource movementSource;
    public bool useExternalSource = false;
    // Start is called before the first frame update
    void Start()
    {
        if (useExternalSource)
        {
            movementSource = new BicycleExternalMovementSource();
        } else
        {
            movementSource = new BicycleSimpleMovementSource();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
