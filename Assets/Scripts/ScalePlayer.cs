using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePlayer : MonoBehaviour
{
    public UnityEngine.UI.Button ScaleButton;
    // public TMPro.TMP_Text uiText;
    public Transform targetObject;
    public float heightIncrement;

    void Awake()
    {
        ScaleButton.onClick.AddListener(Scale);
    }

    void Scale()
    {
        targetObject.position += new Vector3(0, heightIncrement, 0);
    }
}
