using System;
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
    public float DisplayedTemperature;
    public float DisplayedRadiation => GetRadiationLevel();
    public float ParticlesCollected;
    public int MinParticles;
    public int MaxParticles;

    public float NoiseRange;

    public AnimationCurve InputVolumeCorve;
    public AnimationCurve InputVolumeParticlesCurve;
    public AnimationCurve Precision;

    public Environment EnvironmentRef;

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

        ParticlesCollected += Mathf.Max(GetParticlesCollectSpeed() * timeDelta, 0);

        if (DisplayedTemperature >= MaxTemperature)
        {
            Fail = true;
        }
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
        if (ParticlesCollected < MinParticles ||  ParticlesCollected > MaxParticles || Fail)
            return -1;
        
        var precision = Precision.Evaluate(ParticlesCollected / MaxParticles);
        _pres = precision;
        var noise = Mathf.Lerp(0, NoiseRange, UnityEngine.Random.value) * (1 - precision);

        return EnvironmentRef.Radiation * precision + noise;
    }

    public void SetInputVolume(float value)
    {
        InputVolume = value;
    }
}