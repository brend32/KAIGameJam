using System;
using UnityEngine;

public class EnvTesterDevice : MonoBehaviour
{
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
    public AnimationCurve Precision;

    public Environment EnvironmentRef;

    public void Update()
    {
        var timeDelta = Time.deltaTime;
        
        DisplayedTemperature += GetHeatUpSpeed() * timeDelta;
        DisplayedTemperature -= CoolSpeed * timeDelta;
        DisplayedTemperature  = Mathf.Max(DisplayedTemperature, EnvironmentRef.Temperature);

        ParticlesCollected += Mathf.Max(GetParticlesCollectSpeed() * timeDelta, 0);

        if (DisplayedTemperature >= MaxTemperature)
        {
            ParticlesCollected = MaxParticles;
        }
    }

    public float GetParticlesCollectSpeed()
    {
        return InputVolumeCorve.Evaluate(InputVolume) * EnvironmentRef.Radiation * EnvironmentRef.Temperature * ParticlesCollectSpeedCoefficient;
    }

    public float GetHeatUpSpeed()
    {
        return InputVolumeCorve.Evaluate(InputVolume) * HeatUpCoefficient * EnvironmentRef.Radiation;
    }

    public float GetRadiationLevel()
    {
        if (ParticlesCollected < MinParticles ||  ParticlesCollected > MaxParticles)
            return -1;
        
        var precision = Precision.Evaluate(ParticlesCollected);
        var noise = Mathf.Lerp(-NoiseRange, NoiseRange, UnityEngine.Random.value) * precision;

        return EnvironmentRef.Radiation + noise;
    }
}