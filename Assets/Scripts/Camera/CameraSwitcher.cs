using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Tooltip("A camera de índice 0 é considerada a câmera principal.")]
    public Camera[] cameras;
    private int camera_idx = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (!cameras[0].enabled)
        {
            cameras[0].enabled = true;
        }
        for (int i = 1; i < cameras.Length; i++)
        {
            if (cameras[i].enabled)
            {
                cameras[i].enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Simples. Da pro gasto
        if (Input.GetKeyDown(KeyCode.E))
        {
            int tmp = (camera_idx + 1) % cameras.Length;
            cameras[tmp].enabled = true;
            cameras[camera_idx].enabled = false;
            camera_idx = tmp;
        }
    }
}
