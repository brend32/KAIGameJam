using System;
using UnityEngine;

public class DrillDevice : MonoBehaviour
{
    public float SpeedPunish;
    
    public bool Failed;
    public float Depth;
    public float MaxDepth;
    public float NormalizedDepth => Depth / MaxDepth;
    public float Speed;
    public float SpeedDecay;

    public float SelectedPower;
    public float Temperature;
    public float MaxTemperature;

    public float NoResistDepth = 0.2f;
    public float NoResistDrop = 0.2f;

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
        {
            Speed = 0;
            Temperature = 0;
            return;
        }
        
        Speed = ExpDecay(Speed, SelectedPower, SpeedDecay, timeDelta);
        
        var invalidSpeedPunish = InvalidSpeedPunish.Evaluate(InvalidSpeedTime);

        if (Speed > Ground.SpeedMax)
        {
            InvalidSpeedTime += timeDelta;
            invalidSpeedPunish = InvalidSpeedPunish.Evaluate(InvalidSpeedTime) * timeDelta;
        }
        else
        {
            invalidSpeedPunish = 0;
            InvalidSpeedTime = 0;
        }

        SpeedPunish = invalidSpeedPunish;

        if (Depth < MaxDepth && Depth > NoResistDepth)
        {
            Temperature += TemperatureCurve.Evaluate(Speed) * Ground.HeatUpCoefficient * timeDelta;
            Temperature += invalidSpeedPunish;
        }

        Temperature -= CoolSpeed * timeDelta;
        if (Temperature > MaxTemperature)
        {
            Fail();
        }
        
        Temperature = Mathf.Clamp(Temperature, Environment.Temperature, MaxTemperature);

        if (Speed > Ground.SpeedMin || Depth < NoResistDepth)
        {
            if (Depth >= MaxDepth)
            {
                Depth = MaxDepth;
                return;
            }

            var drop = Ground.Drop.Evaluate(Speed);
            if (Depth < NoResistDepth)
            {
                drop = Mathf.Max(drop, NoResistDrop * Speed);
            }
            Depth += drop * timeDelta;
            Temperature -= CoolSpeed * drop * 2 * timeDelta;
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
    
    public void SetSpeed(float speed)
    {
        SelectedPower = speed;
    }
}