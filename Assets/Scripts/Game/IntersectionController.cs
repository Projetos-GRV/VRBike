using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool vertical = false;
    private bool horizontal = false;

    private float timerInitialValue = 0;
    private float timer = 0;
    void Start()
    {
        vertical = Random.value < 0.5f;
        horizontal = !vertical;

        timerInitialValue = 3.0f;
        //timer = timerInitialValue;
        timer = Random.Range(0, timerInitialValue);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            vertical = !vertical;
            horizontal = !horizontal;
            timer = timerInitialValue;
        }
    }

    public bool IsRedLight(Transform vehicle)
    {
        Vector3 forward = vehicle.forward;
        return (Mathf.Abs(Mathf.Round(forward.x)) == 1 && horizontal) || (Mathf.Abs(Mathf.Round(forward.z)) == 1 && vertical);
    }
}
