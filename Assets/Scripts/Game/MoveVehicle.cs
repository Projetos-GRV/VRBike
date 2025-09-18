using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveVehicle : MonoBehaviour
{
    public float speed = 20f;
    public GameObject cityGen = null;

    private bool destroy = false;
    private bool redlight = false; // de momento, so serve para indicar que o objeto itControl existe (evita NullException)
    private bool braking = false;
    private Rigidbody rb;
    private CityGenerator cg;
    private float currSpeed = 0;
    private float scale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider bc = this.AddComponent<BoxCollider>();
        bc.isTrigger = false;

        rb = this.AddComponent<Rigidbody>(); 
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.isKinematic = false;

        this.cg = this.cityGen.GetComponent<CityGenerator>();
        this.currSpeed = this.speed;

        if (cityGen != null)
        {
            scale = cityGen.transform.localScale.x;
        }
        //rb.constraints = RigidbodyConstraints.FreezePositionY;
    }

    // Update is called once per frame
    private float timer = 5f;
    private bool applyForce = true;
    private Vector3 collisionDir = Vector3.zero;
    private IntersectionController itControl = null;
    void FixedUpdate()
    {
        if (this.destroy)
        {
            this.currSpeed = 0;
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
            return;
        }

        this.currSpeed = CheckIfShouldBrake() ? 0 : this.speed;

        if (itControl != null && !itControl.IsRedLight(this.transform))
        {
            itControl = null;
            this.redlight = false;
        }

        rb.MovePosition(transform.position + this.currSpeed * this.scale * Time.fixedDeltaTime * transform.forward);
        if (cityGen != null)
        {
            if (this.transform.position.y < -0.01f)
            {
                this.destroy = true;
                this.applyForce = false;
                this.timer = 0;
            }
        }
        //this.transform.rotation *= Quaternion.AngleAxis(90 * Time.deltaTime, Vector3.up);
    }

    private bool CheckIfShouldBrake()
    {
        if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit carHit, 5.0f))
        {
            if (carHit.collider.CompareTag("Vehicle"))
            {
                return true;
            }
            else
            {
                return this.redlight && itControl.IsRedLight(this.transform);
            }
        }
        return this.redlight && itControl.IsRedLight(this.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Intersection"))
        {
            itControl = other.gameObject.GetComponent<IntersectionController>();
            this.redlight = itControl.IsRedLight(this.transform);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!(collision.gameObject.CompareTag("Road") || collision.gameObject.CompareTag("Intersection")))
        {
            this.destroy = true;
            if (collision.gameObject.CompareTag("Vehicle") || collision.gameObject.CompareTag("PlayerTag"))
            {
                collisionDir = collision.transform.forward;
            }
            else
            {
                collisionDir = this.transform.forward * -1;
            }
        }
    }
}
