using System;
using UnityEngine;

public class CancelParentRotation : MonoBehaviour
{
    public Vector3 extraRotation;

    private Quaternion initialLocalRotation;

    private float _offsetYaw = 0f;
    public float _rotationSpeed = 20f;
    public bool _invertSignal = true;
    public bool _useScript = true;

    Quaternion _lastRotation = Quaternion.identity;

    void Start()
    {
        // Guarda a rota��o local inicial (caso o Player j� esteja orientado de uma forma espec�fica)
        initialLocalRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        if (!_useScript) return;

        // Cancela a rota��o herdada do pai (mant�m posi��o mas ignora rota��o)
        //transform.rotation = Quaternion.identity;

        // Reposiciona o Player onde estaria em rela��o ao pai (posi��o local preservada)
        //transform.position = transform.parent.position + transform.parent.rotation * transform.localPosition;

        // Aplica rota��o inicial + extraRotation
        transform.rotation = Quaternion.Lerp(_lastRotation, initialLocalRotation * Quaternion.Euler(extraRotation), Time.deltaTime * _rotationSpeed);

        _lastRotation = transform.rotation;
    }

    public void SetReference(float offsetYaw)
    {
        _offsetYaw = offsetYaw;
        _lastRotation = transform.rotation;
        initialLocalRotation = transform.localRotation;

        Debug.Log($"[{GetType()}][SetReference] _currentYaw={_offsetYaw}  _lastRotation.eulerAngles={_lastRotation.eulerAngles}   transform.eulerAngles={transform.eulerAngles}");
    }

    public void UpdateRotaion(float currentYaw)
    {
        extraRotation = Vector3.up * (currentYaw);
    }
}
