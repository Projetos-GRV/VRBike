using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePlayer : MonoBehaviour
{
    public UnityEngine.UI.Button ScaleButton;
    public TMPro.TMP_Text uiText;
    public Transform targetObject;
    public Vector3 targetScale;

    void Awake()
    {
        ScaleButton.onClick.AddListener(Scale);
        uiText.text = string.Format("Current height:\n~{0}cm", Mathf.Round(targetObject.localScale.y * 86f / 1.272f + 8));
    }

    void Scale()
    {
        targetObject.localScale += targetScale;
        uiText.text = string.Format("Current height:\n~{0}cm", Mathf.Round(targetObject.localScale.y * 86f / 1.272f + 8));
    }
}
