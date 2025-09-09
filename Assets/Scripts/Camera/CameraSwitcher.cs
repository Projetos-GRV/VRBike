using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Tooltip("A camera de indice 0 eh considerada a câmera principal.")]
    public Camera[] cameras;
    private int cameraIdx = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < cameras.Length; i++)
        {
            if (cameras[i].enabled)
            {
                cameras[i].enabled = false;
            }
        }
        cameras[0].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Simples. Da pro gasto
        if (Input.GetKeyDown(KeyCode.E))
        {
            int tmp = (cameraIdx + 1) % cameras.Length;
            cameras[tmp].enabled = true;
            cameras[cameraIdx].enabled = false;
            cameraIdx = tmp;
        }
    }
}
