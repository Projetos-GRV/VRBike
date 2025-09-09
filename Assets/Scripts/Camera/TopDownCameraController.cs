using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bicycle;

    private Vector3 offset;

    void Start()
    {
        offset = this.transform.position - bicycle.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.position = bicycle.transform.position + offset;
    }
}
