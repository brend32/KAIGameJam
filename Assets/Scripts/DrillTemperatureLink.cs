using System;
using UnityEngine;

public class DrillTemperatureLink : MonoBehaviour
{
    public DrillDevice Tester;
    public TemperatureDisplay Display;

    public void Update()
    {
        Display.Temperature = Tester.Temperature;
    }
}