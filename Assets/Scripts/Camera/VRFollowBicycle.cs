using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFollowBicycle : MonoBehaviour
{
    public GameObject bicycle;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = this.transform.position - bicycle.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.position = bicycle.transform.position + offset;
        this.transform.rotation = bicycle.transform.rotation;
    }
}
