using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Environment", fileName ="Environment")]
public class EnvironmentSO : ScriptableObject
{
    public string Name;
    public GameObject Prefab;
    public Sprite Thumbnail;

}
