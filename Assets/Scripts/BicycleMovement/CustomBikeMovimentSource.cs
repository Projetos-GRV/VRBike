using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using CustomUDPListener;
using UnityEngine;
using UnityEngine.Events;

public class CustomBikeMovimentSource: MonoBehaviour, IBicycleMovementSource
{
    [Header("Network")]
    public int listenPortAngle = 5000;

    [Header("Speed")]
    public bool _useKeyboardForSpeed = false;

    [Header("Rotation")]
    public float _minAngle = -80f;
    public float _maxAngle = 80f;
    public bool _invertSide = false;
    public bool _useKeyboardForRotation = false;

    [Header("Events")]
    public UnityEvent<float> OnMinAngleChanged;
    public UnityEvent<float> OnMaxAngleChanged;
    public UnityEvent<float> OnSpeedChanged;
    public UnityEvent<float> OnAngleChanged;
    public UnityEvent<float> OnSpeedMultiplierChanged;
    public UnityEvent<float> OnHandleSensibility;

    private CustomUDPDataListener dataSource;

    private float speed;
    private float handlebarRotation;
    private Vector2 direction;

    public float _fakeAngle = 0;
    public float _fakeSpeed = 0;

    public bool _useFakeData = false;

    public float GetHandlebarRotation() { return this.handlebarRotation; }
    public Vector2 GetFrontWheelDirection() { return this.direction; }
    public float GetSpeed() { return this.speed; }

    private bool _wKeyPressed = false;

    private string _defaultSpeed = "0";
    private float _rawAngle = 0f;
    private bool _useDefaultSpeed = false;

    // Start is called before the first frame update
    void Start()
    {
        this.speed = 0;
        this.handlebarRotation = 0;
        this.direction = Vector2.zero;
        this.dataSource = new CustomUDPDataListener(this.listenPortAngle);
        bool success = this.dataSource.Init();
        if (!success)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("It was not possible to open the UDP ports.");
            }
        }
    }

    public void SetDefaultSpeed(float speed)
    {
        _defaultSpeed = speed.ToString();
        _useDefaultSpeed = speed == 0.0 ? false : true;
    }

    // Update is called once per frame
    void Update()
    {
        string data = _useFakeData ? $"{_fakeAngle};{_fakeSpeed}" : this.dataSource.GetData();

        //Debug.Log(data);

        string[] dataSplit = data.Split(';');



        if (dataSplit.Length != 2) return;

        string angleStr = dataSplit[0];
        string speedStr = _useDefaultSpeed ? _defaultSpeed : dataSplit[1];
        speedStr = speedStr.Replace(',', '.');
        angleStr = angleStr.Replace(",", ".");


        float speed = 0;

        var angle = HandleAngle;
        var newRawAngle = float.Parse(angleStr, NumberStyles.Number, CultureInfo.InvariantCulture);

        if (Mathf.Abs(_rawAngle - newRawAngle) >= AngleThreshold)
        {
            _rawAngle = newRawAngle;

            angle = AppUtils.Map(_rawAngle, MinAngle, MaxAngle, -1, 1);

            angle = angle * _maxAngle;

            angle = Mathf.Abs((MaxAngle - MinAngle) / 2f - _rawAngle) > AngleThreshold ? angle : 0;
        }

        speed = float.Parse(speedStr, NumberStyles.Number, CultureInfo.InvariantCulture) * SpeedMultiplier;

        //Debug.Log($"Final Angle {angle}  |  Speed {speed}");

        this.handlebarRotation = angle;
        this.speed = speed;

        Vector3 tmp = Vector3.forward;
        tmp = Quaternion.AngleAxis(this.handlebarRotation, Vector3.up) * tmp;
        this.direction.x = tmp.x;
        this.direction.y = tmp.z;

        OnAngleChanged?.Invoke(_rawAngle);
        OnSpeedChanged?.Invoke(Speed);
    }

    private void OnDisable()
    {
        dataSource.Halt();
    }

    public void SaveMinValue()
    {
        MinAngle = _rawAngle;

        OnMinAngleChanged?.Invoke(_rawAngle);
    }

    public void SaveMaxValue()
    {
        MaxAngle = _rawAngle;

        OnMaxAngleChanged?.Invoke(_rawAngle);
    }

    public void HandleAngleThreshold(string newValue)
    {
        var clenedStr = newValue.Trim();
        AngleThreshold = float.Parse(clenedStr, NumberStyles.Number, CultureInfo.InvariantCulture);

        OnHandleSensibility?.Invoke(AngleThreshold);
    }

    public void HandleSpeedMultplierChanged(string newValue)
    {
        var clenedStr = newValue.Trim().Replace(",", ".");

        Debug.Log($"Multiplier  {newValue}  {clenedStr} {float.Parse(clenedStr, NumberStyles.Number, CultureInfo.InvariantCulture)}");
        SpeedMultiplier = float.Parse(clenedStr, NumberStyles.Number, CultureInfo.InvariantCulture);

        OnSpeedMultiplierChanged?.Invoke(SpeedMultiplier);
    }

    public float MinAngle { get; set; }
    public float MaxAngle { get; set; }
    public float AngleThreshold { get; set; }

    public float SpeedMultiplier { get; set; }
    public float RawSpeed => speed;
    public float Speed => speed * SpeedMultiplier;

    public float RawAngle => _rawAngle;
    public float HandleAngle => handlebarRotation;
}
