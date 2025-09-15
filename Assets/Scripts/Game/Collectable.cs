using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public bool disableAnimation = false;
    void Start()
    {
        if (this.disableAnimation)
        {
            GetComponent<Animator>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTag"))
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("Collect");
            }
        }
        //collision.gameObject.SetActive(false); // Now I am become death... the destroyer of worlds
    }
}
