using System;
using UnityEngine;

public class TemperatureDisplay : MonoBehaviour
{
    public Lamp[] Lamps;
    public float Temperature;

    public void Update()
    {
        var index = ConvertTemperatureToIndex(Temperature);

        for (int i = 0; i < Lamps.Length; i++)
        {
            Lamps[i].SetValue(i < index);
        }
    }

    public static float ConvertTemperatureToIndex(float temperature)
    {
        return Mathf.FloorToInt(temperature / 20);
    }

    public static float ConvertIndexToTemperature(float temperature)
    {
        return temperature * 20;
    }
}