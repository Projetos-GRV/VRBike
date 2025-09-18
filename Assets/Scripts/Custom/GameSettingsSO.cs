using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Game Settings")]
public class GameSettingsSO : ScriptableObject
{
    public float MinRawAngle;
    public float MaxRawAngle;
    public float BaseSpeedMultiplier;
    public float HandleSensibility;
}
