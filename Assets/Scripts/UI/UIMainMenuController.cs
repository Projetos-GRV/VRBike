using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputHandlerThreshold;
    [SerializeField] private TMP_InputField _inputSpeedMultiplier;


    public void UpdateHandleThresholdDisplay(float value) => _inputHandlerThreshold.SetTextWithoutNotify(value.ToString());
    public void UpdateSpeedMultiplierDisplay(float value) => _inputSpeedMultiplier.SetTextWithoutNotify(value.ToString());
}
