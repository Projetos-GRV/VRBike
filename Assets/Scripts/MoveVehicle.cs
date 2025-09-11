using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVehicle : MonoBehaviour
{
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + this.transform.forward, this.speed * Time.deltaTime);

        this.transform.rotation *= Quaternion.AngleAxis(90 * Time.deltaTime, Vector3.up);
    }
}
