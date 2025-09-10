using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UDPListener;

public class BicycleUDPMovementSource : MonoBehaviour, IBicycleMovementSource
{
    public int listenPortSpeed = 8000;
    public int listenPortAngle = 8001;
    [Tooltip("1 - eixo X; 2 - eixo Y; 3 - eixo Z")]
    public int gyroAxis = 3;

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
        this.handlebarRotation = 0;
        this.direction = Vector2.zero;
        //this.direction = Vector2.forward;
        this.dataSource = new UDPDataListener(this.listenPortSpeed, this.listenPortAngle);
        bool success = this.dataSource.Init();
        if (!success)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("It was not possible to open the UDP ports.");
            }
        }
        if (this.gyroAxis < 0 || this.gyroAxis > 3)
        {
            this.gyroAxis = 3;
        }
    }

    public bool _useFakeData = true;
    public float _angle = 0;
    public float _speed = 0;

    // Update is called once per frame
    void Update()
    {
        string angleStr = this.dataSource.GetAngleData();
        string speedStr = this.dataSource.GetSpeedData();

        if (_useFakeData)
        {
            angleStr = $"0;0;{_angle}";
            speedStr = _speed.ToString("0.00");
        }

        if (Debug.isDebugBuild)
        {
            Debug.Log(angleStr);
            Debug.Log(speedStr);
        }

        string[] gyroValuesStr = angleStr.Trim().Split(';');
        float[] gyroValues = new float[gyroValuesStr.Length];
        
        // talvez só um eixo seja relevante.... 
        for (int i = 0; i < gyroValuesStr.Length; i++)
        {
            gyroValues[i] = float.Parse(gyroValuesStr[i], CultureInfo.InvariantCulture);
        }

        //float deltaDist = gyroValues[0] * this.dataSource.GetAngleTime();

        this.handlebarRotation = -gyroValues[this.gyroAxis - 1]; // TODO - considerar momentos em que o eixo parece zerar sem motivo aparente.
        this.speed = float.Parse(speedStr.Trim()); // TODO - considerar que a velocidade virá em pulsos (0 - 1). Considerar intervalo entre pulsos.

        Vector3 tmp = Vector3.forward;
        tmp = Quaternion.AngleAxis(this.handlebarRotation, Vector3.up) * tmp;
        this.direction.x = tmp.x;
        this.direction.y = tmp.z;
    }
}
