using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePlayer : MonoBehaviour
{
    public UnityEngine.UI.Button ScaleButton;
    public TMPro.TMP_Text uiText;
    public Transform targetObject;
    public Vector3 scaleIncrement;

    void Awake()
    {
        ScaleButton.onClick.AddListener(Scale);
        // 86f (em centimetros) ~ aproximadamente a altura do banco da bike real
        // 1.272f ~ aproximadamente a altura do banco na Unity
        // + 8 (em centimetros) ~ para compensar a altura da testa em relacao aos olhos (medida retirada de fontes online)
        // a matematica pode estar errada. sinta-se livre para corrigi-la
        uiText.text = string.Format("Altura aproximada:\n~{0}cm", Mathf.Round(targetObject.localScale.y * 86f / 1.272f + 8));
    }

    void Scale()
    {
        // incremento
        // 0.0147907f ~ aproximadamente 1cm
        targetObject.localScale += scaleIncrement;
        uiText.text = string.Format("Altura aproximada:\n~{0}cm", Mathf.Round(targetObject.localScale.y * 86f / 1.272f + 8));
    }
}
