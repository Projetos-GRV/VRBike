using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResetTriggerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _camera;

    [Header("Configurações")]
    public float _maxDistance = 10f;          // Distância máxima para detectar
    public float _gazeTime = 2f;              // Tempo necessário fixando o olhar

    [Header("Eventos")]
    public UnityEvent onGazeTrigger;         // Evento disparado ao fixar o olhar

    [Header("UI")]
    [SerializeField] private GameObject _uiInstance;
    [SerializeField] private GameObject _uiView;
    [SerializeField] private Slider _sliderProgress;

    private bool _leftTrigger = false;
    private bool _rightTrigger = false;

    private float _gazeTimer = 0f;            // Contador interno

    private void Awake()
    {
        _uiView.SetActive(false);
    }
    /*
    void Update()
    {
        // Faz o raycast a partir da posição da cabeça, na direção do olhar
        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _maxDistance))
        {
            // Verifica se o objeto atingido é este UI
            if (hit.collider.gameObject == _uiInstance)
            {
                _uiView.SetActive(true);

                _gazeTimer += Time.deltaTime;

                _sliderProgress.value = _gazeTimer / _gazeTime;

                Debug.Log($"{_gazeTimer} {_gazeTime}");
                if (_gazeTimer >= _gazeTime)
                {
                    Debug.Log("Trigger");
                    onGazeTrigger?.Invoke();
                    _gazeTimer = 0f; // reset para evitar múltiplos disparos
                }
                return;
            }
        }


        // Se não estiver olhando, reseta o contador
        _gazeTimer = 0f;
        _sliderProgress.value = 0f;
        _uiView.SetActive(false);
    }
    */
    public void HandleLeftTrigger(bool newValue)
    {
        _leftTrigger = newValue;

        Debug.Log($"Left  {newValue}");

        if (_leftTrigger && _rightTrigger)
        {
            onGazeTrigger?.Invoke();

            _leftTrigger = _rightTrigger = false;
        }
    }

    public void HandleRightTrigger(bool newValue)
    {
        _rightTrigger = newValue;

        Debug.Log($"Right  {newValue}");

        if (_leftTrigger && _rightTrigger)
        {
            onGazeTrigger?.Invoke();

            _leftTrigger = _rightTrigger = false;
        }
    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawLine(_camera.position, _camera.position + _camera.forward * _maxDistance);
    }
}
