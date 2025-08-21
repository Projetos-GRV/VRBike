using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

using UDPListener;

public class BicycleUDPMovementSource : MonoBehaviour, IBicycleMovementSource
{
    public int listenPortSpeed = 8000;
    public int listenPortAngle = 8001;

    private UDPDataListener dataSource;

    private float speed;
    private float handlebarRotation;
    private Vector2 direction;

    public float GetHandlebarRotation() { return this.handlebarRotation; }
    public Vector2 GetFrontWheelDirection() { return this.direction; }
    public float GetSpeed() { return this.speed; }

    // Start is called before the first frame update
    void Start()
    {
        this.speed = 0;
        this.direction = Vector2.zero;
        this.handlebarRotation = 0;
        this.dataSource = new UDPDataListener(this.listenPortSpeed, this.listenPortAngle);
        bool success = this.dataSource.Init();
        if (!success)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("It was not possible to open the UDP ports.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        string angleStr = this.dataSource.GetAngleData();
        string speedStr = this.dataSource.GetSpeedData();

        string[] gyroValuesS = angleStr.Trim().Split(';');
        float[] gyroValuesF = new float[gyroValuesS.Length];

        for (int i = 0; i < gyroValuesS.Length; i++)
        {
            gyroValuesF[i] = float.Parse(gyroValuesS[i]);
        }

        this.handlebarRotation = 0;
        this.speed = float.Parse(speedStr.Trim());
    }
}
