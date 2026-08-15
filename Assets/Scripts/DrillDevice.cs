using System;
using UnityEngine;

public class DrillDevice : MonoBehaviour
{
    public bool Failed;
    public float Depth;
    public float Speed;
    public float SpeedDecay;

    public float SelectedPower;
    public float Temperature;
    public float MaxTemperature;

    public AnimationCurve InvalidSpeedPunish;
    public AnimationCurve TemperatureCurve;
    public float CoolSpeed;
    public float InvalidSpeedTime;
    public AnimationCurve DropSpeed;

    public Environment Environment;
    public GroudSO Ground => Environment.Ground;

    public void Update()
    {
        var timeDelta = Time.deltaTime;

        if (Failed)
            return;
        
        Speed = ExpDecay(Speed, SelectedPower, SpeedDecay, timeDelta);
        
        var invalidSpeedPunish = InvalidSpeedPunish.Evaluate(InvalidSpeedTime);

        if (Speed > Ground.SpeedMax)
        {
            InvalidSpeedTime += timeDelta;
            invalidSpeedPunish -= InvalidSpeedPunish.Evaluate(InvalidSpeedTime);
        }
        else
        {
            invalidSpeedPunish = 0;
            InvalidSpeedTime = 0;
        }
        
        Temperature += TemperatureCurve.Evaluate(Speed) * Ground.HeatUpCoefficient * timeDelta;
        Temperature += invalidSpeedPunish;
        Temperature -= CoolSpeed * timeDelta;
        if (Temperature > MaxTemperature)
        {
            Fail();
        }
        
        Temperature = Mathf.Clamp(Temperature, Environment.Temperature, MaxTemperature);

        if (Speed > Ground.SpeedMin)
        {
            Depth += DropSpeed.Evaluate(Speed) * timeDelta;
        }
    }
    
    public static float ExpDecay(float a, float b, float decay, float deltaTime)
    {
        return b + (a - b) * Mathf.Exp(-decay * deltaTime);
    }

    public void Fail()
    {
        Failed = true;
    }
}