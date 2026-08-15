using System;
using UnityEngine;

public class EnvTemperatureLink : MonoBehaviour
{
    public EnvTesterDevice Tester;
    public TemperatureDisplay Display;

    public void Update()
    {
        Display.Temperature = Tester.DisplayedTemperature;
    }
}