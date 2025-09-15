using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveVehicle : MonoBehaviour
{
    public float speed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider bc = this.AddComponent<BoxCollider>();
        Rigidbody rb = this.AddComponent<Rigidbody>();
        bc.isTrigger = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + this.transform.forward, this.speed * Time.deltaTime);

        this.transform.rotation *= Quaternion.AngleAxis(90 * Time.deltaTime, Vector3.up);
    }
}
