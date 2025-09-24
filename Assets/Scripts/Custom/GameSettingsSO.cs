using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Game Settings")]
public class GameSettingsSO : ScriptableObject
{
    [Header("Bike Parameters")]
    public float MinRawAngle;
    public float MaxRawAngle;
    public float BaseSpeedMultiplier;
    public float HandleSensibility;

    [Header("Game Parameters")]
    public int GameTime = 60;
}
