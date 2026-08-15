using System;
using UnityEngine;

public class RadiationView : MonoBehaviour
{
    public float FromRoation;
    public float ToRoation;
    public AnimationCurve Decay;
    public float Value;
    [Range(0, 1)]
    public float TargetValue;

    public void Update()
    {
        Value = ExpDecay(Value, TargetValue, Decay.Evaluate(Mathf.Abs(TargetValue - Value)),  Time.deltaTime);
        
        transform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(FromRoation, ToRoation, Mathf.Clamp01(Value)));
    }
    
    public static float ExpDecay(float a, float b, float decay, float deltaTime)
    {
        return b + (a - b) * Mathf.Exp(-decay * deltaTime);
    }

    public void OnValidate()
    {
        //transform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(FromRoation, ToRoation, Mathf.Clamp01(TargetValue)));
    }
}