using System;
using DefaultNamespace;
using UnityEngine;

public class EnvTesterDevice : MonoBehaviour
{
    public float _pres;

    public bool Fail;
    
    public float MaxTemperature;
    public float CoolSpeed;
    public float HeatUpCoefficient;
    public float ParticlesCollectSpeedCoefficient;

    public float InputVolume;
    public float TestInput;
    public float TestInputDecay;
    public float MinInput;
    public float DisplayedTemperature;
    public float DisplayedRadiation => GetRadiationLevel();

    public float NoiseRange;

    public AnimationCurve InputVolumeCorve;
    public AnimationCurve InputVolumeParticlesCurve;
    public AnimationCurve Precision;

    public Environment EnvironmentRef;
    public StatusPicker StatusPicker;

    public void Update()
    {
        var timeDelta = Time.deltaTime;

        if (Fail)
        {
            DisplayedTemperature = 0;
            return;
        }
        
        DisplayedTemperature += GetHeatUpSpeed() * timeDelta;
        DisplayedTemperature -= CoolSpeed * timeDelta;
        DisplayedTemperature  = Mathf.Max(DisplayedTemperature, EnvironmentRef.Temperature);

        TestInput = ExpDecay(TestInput, InputVolume, TestInputDecay, timeDelta);

        if (DisplayedTemperature >= MaxTemperature)
        {
            Fail = true;
        }
    }
    
    public static float ExpDecay(float a, float b, float decay, float deltaTime)
    {
        return b + (a - b) * Mathf.Exp(-decay * deltaTime);
    }

    public float GetParticlesCollectSpeed()
    {
        return GetInputParticlesVolume() * ParticlesCollectSpeedCoefficient;
    }

    public float GetHeatUpSpeed()
    {
        return GetInputVolume() * HeatUpCoefficient;
    }

    public float GetInputVolume()
    {
        return Mathf.Max(0, InputVolumeCorve.Evaluate(InputVolume));
    }
    
    public float GetInputParticlesVolume()
    {
        return Mathf.Max(0, InputVolumeParticlesCurve.Evaluate(InputVolume));
    }

    public float GetRadiationLevel()
    {
        if (Fail || TestInput < MinInput)
        {
            StatusPicker.Value = 0;
            return -1;
        }
        
        var precision = Precision.Evaluate(TestInput);
        _pres = precision;
        var noise = Mathf.Lerp(0, NoiseRange, UnityEngine.Random.value) * (1 - precision);

        StatusPicker.Value = precision;

        return EnvironmentRef.Radiation * precision + noise;
    }

    public void SetInputVolume(float value)
    {
        InputVolume = value;
    }
}