using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class ResetPlayerController : MonoBehaviour
{
    [SerializeField] private Transform _cameraReference;

    [SerializeField] private SyncVirtualObjectController _temp;
    [SerializeField] private CustomBikeMovement _customBikeMovement;

    [Header("Parameters")]
    [SerializeField] private int _timeToReset = 3;
    [SerializeField] private float _uiDistanceFromHMD = 1.5f;
    [SerializeField] private float _timeToConfirm = 5f;
    [SerializeField] private string _confirmationDescription = "Deseja redefinir a posição da bicicleta?";

    [Header("UI")]
    [SerializeField] private GameObject _uiView;
    [SerializeField] private TextMeshProUGUI _txtTimer;
    [SerializeField] private UIConfirmationScreenController _confirmationScreenController;

    private Coroutine _resetCoroutine;
    private bool _inResetPreocess = false;


    public void StartResetProcess(Action onCompleted, Action onError)
    {
        /*
        if (_resetCoroutine != null)
            StopCoroutine(_resetCoroutine);


        _confirmationScreenController.GetConfirmation(_confirmationDescription, _timeToConfirm, () =>
        {
            StartCoroutine(ResetCoroutine());
        }, null);

        */

        if (_customBikeMovement.IsMoving)
        {
            onError?.Invoke();
            return;
        }

        if (_resetCoroutine != null)
            StopCoroutine(_resetCoroutine);

        StartCoroutine(ResetCoroutine(onCompleted));
    }

    private IEnumerator ResetCoroutine(Action onCompleted)
    {
        _inResetPreocess = true;

        _uiView.SetActive(true);

        yield return null;

        float time = _timeToReset;

        AppUtils.SetPositionInFront(_uiView.transform, _cameraReference, _uiDistanceFromHMD);

        do
        {
            _txtTimer.text = ((int)time + 1).ToString("0");
            time -= Time.deltaTime;

            _temp.UpdateBikePosition();

            yield return null;
        } while (time > 0);

        _uiView.SetActive(false);

        _inResetPreocess = false;

        onCompleted?.Invoke();  
    }
    /*
    private void ResetPlayerPosition()
    {
        if (xrOrigin != null && targetPoint != null)
        {
            // Reposiciona mantendo o offset correto entre XR Origin e Camera
            xrOrigin.MoveCameraToWorldLocation(targetPoint.position);
            xrOrigin.MatchOriginUpCameraForward(targetPoint.up, targetPoint.forward);
        }

        _cameraOffsetChild.localPosition = _cameraReference.localPosition * -1;
    }*/

    public bool InResetProcess => _inResetPreocess;
}
