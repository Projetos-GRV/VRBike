using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Research Group", fileName = "ResearchGroup_")]
public class ResearchGroupSO : ScriptableObject
{
    [SerializeField] private Sprite _banner;

    public Sprite Banner => _banner;
}
