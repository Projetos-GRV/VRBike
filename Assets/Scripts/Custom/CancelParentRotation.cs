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
        // Guarda a rotação local inicial (caso o Player já esteja orientado de uma forma específica)
        initialLocalRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        if (!_useScript) return;

        // Cancela a rotação herdada do pai (mantém posição mas ignora rotação)
        //transform.rotation = Quaternion.identity;

        // Reposiciona o Player onde estaria em relação ao pai (posição local preservada)
        //transform.position = transform.parent.position + transform.parent.rotation * transform.localPosition;

        // Aplica rotação inicial + extraRotation
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
