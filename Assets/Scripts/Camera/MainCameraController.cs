using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleCameraController : MonoBehaviour
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
        // TODO - Funciona, mas eh um pouco falho. A bicicleta rotaciona em relacao a um ponto diferente do da camera.
        this.transform.forward = bicycle.transform.forward;
    }
}
