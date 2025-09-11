using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameManagerController : MonoBehaviour
{
    [SerializeField] private GameObject _view;

    public void SetActive(bool isActive) => _view.SetActive(isActive);
}
