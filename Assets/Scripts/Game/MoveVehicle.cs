using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveVehicle : MonoBehaviour
{
    public float speed = 20f;
    public GameObject cityGen = null;

    private bool destroy = false;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider bc = this.AddComponent<BoxCollider>();
        bc.isTrigger = false;

        rb = this.AddComponent<Rigidbody>(); 
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //rb.constraints = RigidbodyConstraints.FreezePositionY;
    }

    // Update is called once per frame
    private float timer = 5f;
    private bool applyForce = true;
    private Vector3 collisionDir = Vector3.zero;
    void FixedUpdate()
    {
        if (this.destroy)
        {
            // Afasta o carro do objeto com o qual este colidiu... pode ser removido se julgado bobo ou desnecessario
            // ... ou ajustado para ser melhor
            // forças absolutamente arbitrárias
            if (applyForce)
            {
                rb.AddForce(25 * collisionDir + new Vector3(0, 100, 0), ForceMode.Force);
                applyForce = false;
            }
            timer -= Time.fixedDeltaTime;
            if (this.timer <= 0)
            {
                this.transform.parent = null;
                Destroy(this.gameObject);
            }
        }
        float scale = 1f;
        if (cityGen != null)
        {
            scale = cityGen.transform.localScale.x;
        }
        //this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + this.transform.forward, this.speed * Time.fixedDeltaTime);
        rb.MovePosition(transform.position + speed * scale * Time.fixedDeltaTime * transform.forward);
        if (cityGen != null)
        {
            CityGenerator cg = this.cityGen.GetComponent<CityGenerator>();
            Vector3 chunk = this.transform.position;
            chunk.x = Mathf.RoundToInt(chunk.x / cg.stride);
            chunk.y = 0f;
            chunk.z = Mathf.RoundToInt(chunk.z / cg.stride);
            if (!cg.IsChunkLoaded(chunk))
            {
                Debug.Log("Unloaded vehicle script");
            }
        }
        //this.transform.rotation *= Quaternion.AngleAxis(90 * Time.deltaTime, Vector3.up);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Road"))
        {
            destroy = true;
            this.speed = 0;
            if (collision.gameObject.CompareTag("Vehicle") || collision.gameObject.CompareTag("PlayerTag"))
            {
                collisionDir = collision.transform.forward;
            }
            else
            {
                collisionDir = this.transform.forward * -1;
            }
            Debug.Log("Collision!!");
        }
    }
}
