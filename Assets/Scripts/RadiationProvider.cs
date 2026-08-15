using UnityEngine;

public class RadiationProvider : MonoBehaviour
{
    public float Point;
    public AnimationCurve Decay;
    public float Radiation;
    
    public void SetPoint(float value)
    {
        Point = value;
    }

    public float GetRadiation(Vector3 position)
    {
        var distance = Mathf.Abs(PositionToProgress(position) - Point);

        return Decay.Evaluate(distance * distance) * Radiation;
    }

    public float PositionToProgress(Vector3 position)
    {
        return position.x;
    }
}