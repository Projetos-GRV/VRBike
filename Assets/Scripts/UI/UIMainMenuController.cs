using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputHandlerThreshold;
    [SerializeField] private TMP_InputField _inputSpeedMultiplier;
    [SerializeField] private TextMeshProUGUI _txtMinValue;
    [SerializeField] private TextMeshProUGUI _txtMaxValue;

    public void UpdateMinValueDisplay(float value) => _txtMinValue.text = $"Min Value\n({((int)value)})";
    public void UpdateMaxValueDisplay(float value) => _txtMaxValue.text = $"Max Value\n({((int)value)})";

    public void UpdateHandleThresholdDisplay(float value) => _inputHandlerThreshold.SetTextWithoutNotify(value.ToString());
    public void UpdateSpeedMultiplierDisplay(float value) => _inputSpeedMultiplier.SetTextWithoutNotify(value.ToString());
}
