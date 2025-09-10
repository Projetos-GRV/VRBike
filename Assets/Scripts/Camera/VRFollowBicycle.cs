using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFollowBicycle : MonoBehaviour
{
    public Transform bicycle;

    private Vector3 offset;
    private Rigidbody rb;
    private Rigidbody otherrb;

    // Start is called before the first frame update
    void Start()
    {
        // this.rb = GetComponent<Rigidbody>();
        // this.otherrb = bicycle.GetComponent<Rigidbody>();
        // offset = this.rb.position - this.otherrb.position;
        SetOffset();
    }

    public void SetOffset()
    {
        offset = this.transform.position - this.bicycle.position;
        offset.y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 followVelocity = Vector3.zero;
        // Vector3 desiredPos = this.otherrb.position + offset;
        //this.transform.position = Vector3.SmoothDamp(
        //    this.transform.position,
        //    desiredPos,
        //    ref followVelocity,
        //    0.05f
        //);
        // this.transform.position = Vector3.Lerp(this.transform.position, desiredPos, 30f * Time.deltaTime);
        // this.rb.MovePosition(Vector3.Lerp(this.rb.position, desiredPos, 30f * Time.fixedDeltaTime));

        // Quaternion desiredRot = this.otherrb.rotation;
        //this.transform.rotation = SmoothDampRotation(
        //    this.transform.rotation,
        //    desiredRot,
        //    0.05f
        //);
        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, desiredRot, 30f * Time.deltaTime);    
        // this.rb.MoveRotation(Quaternion.Slerp(this.rb.rotation, desiredRot, 30f * Time.fixedDeltaTime));

        Vector3 desiredPos = bicycle.position;
        desiredPos.y = 0;
        this.transform.position = desiredPos + offset;
        this.transform.rotation = bicycle.rotation;
    }
}
