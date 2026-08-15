using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GroudSO")]
public class GroudSO : ScriptableObject
{
    public GroundType Type;
    public float HeatUpCoefficient;

    [Range(0, 1)]
    public float SpeedMin;
    [Range(0, 1)]
    public float SpeedMax;
    public AnimationCurve Drop;
}