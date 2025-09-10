using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class UISpeedometerController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CustomBikeMovimentSource _dataSource;

    [Header("Virtual Buttons")]
    [SerializeField] private FingerButtonController _btnToggleSpeed;
    [SerializeField] private FingerButtonController _btnDecreaseSpeed;
    [SerializeField] private FingerButtonController _btnIncreaseSpeed;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _txtSpeed;
    [SerializeField] private TextMeshProUGUI _txtMinAngle;
    [SerializeField] private TextMeshProUGUI _txtAngle;
    [SerializeField] private TextMeshProUGUI _txtMaxAngle;
    [SerializeField] private TextMeshProUGUI _txtSpeedMultiplier;
    
    private int _currentSpeed = 1;
    private bool _isContantSpeedEnabled = false;

    private void Awake()
    {
        _dataSource.OnAngleChanged.AddListener(result => HandleValueChanged(result, _txtAngle));
        _dataSource.OnSpeedChanged.AddListener(result => HandleValueChanged(result.ToString("0.00"), _txtSpeed));
        _dataSource.OnMinAngleChanged.AddListener(result => HandleValueChanged(result, _txtMinAngle));
        _dataSource.OnMaxAngleChanged.AddListener(result => HandleValueChanged(result, _txtMaxAngle));
        _dataSource.OnSpeedMultiplierChanged.AddListener(result => HandleValueChanged("x" + result.ToString("0.00"), _txtSpeedMultiplier));

        _btnToggleSpeed.OnFingerEnter.AddListener(() => ToggleConstantSpeed());
        _btnDecreaseSpeed.OnFingerEnter.AddListener(() => DecreaseSpeed());
        _btnIncreaseSpeed.OnFingerEnter.AddListener(() => IncreaseSpeed());
    }

    private void Start()
    {
        Debug.Log($"{_dataSource.RawAngle} {_dataSource.Speed} {_dataSource.MinAngle} {_dataSource.MaxAngle} {_dataSource.SpeedMultiplier}");

        HandleValueChanged(_dataSource.RawAngle, _txtAngle);
        HandleValueChanged(_dataSource.Speed, _txtSpeed);
        HandleValueChanged(_dataSource.MinAngle, _txtMinAngle);
        HandleValueChanged(_dataSource.MaxAngle, _txtMaxAngle);
        HandleValueChanged("x" + _dataSource.SpeedMultiplier.ToString("0.00"), _txtSpeedMultiplier);
    } 

    public void ToggleConstantSpeed()
    {
        _isContantSpeedEnabled = !_isContantSpeedEnabled;

        if (_isContantSpeedEnabled)
        {
            _currentSpeed = 1;
            _dataSource.SetDefaultSpeed(_currentSpeed);
        }
        else
        {
            _dataSource.SetDefaultSpeed(0);
        }
    }

    public void IncreaseSpeed()
    {
        if (!_isContantSpeedEnabled) return;

        _currentSpeed += 1;

        _dataSource.SetDefaultSpeed(_currentSpeed);
    }

    public void DecreaseSpeed()
    {
        if (!_isContantSpeedEnabled) return;

        _currentSpeed = Mathf.Max(_currentSpeed - 1, 0);

        _dataSource.SetDefaultSpeed(_currentSpeed);
    }


    public void HandleValueChanged(float value, TextMeshProUGUI textComponent) => textComponent.text = value.ToString("0");
    public void HandleValueChanged(string value, TextMeshProUGUI textComponent) => textComponent.text = value;
}
