using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameRandom : MonoBehaviour
{
    public GroudSO[] GroundTypes;
    public int[] Temperatures;
    [Range(0, 1)]
    public float BoundOffset;
    public float RadiationMin;
    public float RadiationMax;
    
    public RadiationProvider RadiationProvider;
    public Environment Environment;

    public void Start()
    {
        RadiationProvider.SetPoint(Mathf.Lerp(BoundOffset, 1 - BoundOffset, Random.value));
        RadiationProvider.Radiation = Random.Range(RadiationMin, RadiationMax);
        Environment.Temperature = Temperatures[Random.Range(0, Temperatures.Length)] * 20;
        Environment.Ground = GroundTypes[Random.Range(0, GroundTypes.Length)];
    }
}