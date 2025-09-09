using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _menuActionReference;
    [SerializeField] private Transform _playerFootPivot;
    [SerializeField] private Transform _origin;
    [SerializeField] private GameObject _instance;
    [SerializeField] private Transform _cameraReference;

    [Header("UI")]
    [SerializeField] private GameObject _view;
    [SerializeField] private UIListDisplayController _environmentListController;

    [Header("Parameters")]
    [SerializeField] private List<EnvironmentSO> _environmentsSO;


    private EnvironmentSO _currentEnvironmentSelected;

    private void Awake()
    {
        _menuActionReference.action.performed += Action_performed;
        
        _view.SetActive(false);
    }

    private void Start()
    {
        transform.parent = _cameraReference;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        _environmentListController.Init(_environmentsSO, HandleEnvironmentSelected);
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        _view.SetActive(!_view.activeSelf);
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

    public void TurnOnMenu(bool _)
    {
        _view.SetActive(true);
    }

    public void TurnOffMenu(bool _)
    {
        _view.SetActive(false);
    }
}
