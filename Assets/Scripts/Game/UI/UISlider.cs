using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISliderTimer : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _imgBar;

    private void UpdateSliderColor()
    {
        float max = _slider.maxValue;
        float value = _slider.value;
        float t = 1f - (value / max); // 0 = início (verde), 1 = fim (vermelho)

        Color color;

        if (t < 0.5f)
        {
            // Verde para Amarelo (primeiro e segundo terço)
            // t de 0 a 0.5 => 0 a 1
            float lerp = t / 0.5f;
            color = Color.Lerp(Color.green, Color.yellow, lerp);
        }
        else
        {
            // Amarelo para Vermelho (segundo e terceiro terço)
            // t de 0.5 a 1 => 0 a 1
            float lerp = (t - 0.5f) / 0.5f;
            color = Color.Lerp(Color.yellow, Color.red, lerp);
        }

        _imgBar.color = color;
    }

    public void UpdateTime(float normalizedTime)
    {
        _slider.value = normalizedTime;
        
        UpdateSliderColor();
    }
}
