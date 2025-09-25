using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;


public class MainMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerFootPivot;
    [SerializeField] private Transform _origin;
    [SerializeField] private GameObject _instance;
    [SerializeField] private Transform _cameraReference;

    [Header("UI")]
    [SerializeField] private GameObject _view;
    [SerializeField] private GameObject _animationView;
    [SerializeField] private UIListDisplayController _environmentListController;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    [Header("Parameters")]
    [SerializeField] private List<EnvironmentSO> _environmentsSO;


    private EnvironmentSO _currentEnvironmentSelected;

    private void Awake()
    {
        _view.SetActive(false);
        _animationView.SetActive(false);
    }

    private void Start()
    {
        transform.parent = _cameraReference;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        //_environmentListController.Init(_environmentsSO, HandleEnvironmentSelected);
    }

    private void HandleEnvironmentSelected(object item, GameObject instance)
    {
        _currentEnvironmentSelected = item as EnvironmentSO;
    }

    public void ChangeEnvironment()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }

        _instance = Instantiate(_currentEnvironmentSelected.Prefab, _origin);

        _instance.transform.localPosition = Vector3.zero;
        _instance.transform.localRotation = Quaternion.identity;

        _playerFootPivot.localPosition = Vector3.zero;
        _playerFootPivot.localRotation = Quaternion.identity;
    }

    public void HandleStartWaiting()
    {
        if (_view.activeSelf) return;
        Debug.Log("HandleStartWaiting");
        _animator.SetBool("Wait", true);
    }

    public void HandleStopWaiting()
    {
        Debug.Log("HandleStopWaiting");
        _animator.SetBool("Wait", false);
        _animationView.SetActive(false);
    }

    public void TurnOnMenu(bool _)
    {
        _view.SetActive(true);
        _animator.SetBool("Wait", false);
    }

    public void TurnOffMenu(bool _)
    {
        _view.SetActive(false);
    }

    public void ToggleView()
    {
        _view.SetActive(!_view.activeSelf);

        _animationView.SetActive(false);
    }
}
