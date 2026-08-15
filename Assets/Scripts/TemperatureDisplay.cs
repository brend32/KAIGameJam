using System;
using UnityEngine;
using UnityEngine.Events;

public class TemperatureDisplay : MonoBehaviour
{
    public UnityEvent LastPointOn;
    public UnityEvent LastPointOff;
    public Lamp[] Lamps;
    public float Temperature;

    public void Update()
    {
        var index = ConvertTemperatureToIndex(Temperature);

        for (int i = 0; i < Lamps.Length; i++)
        {
            Lamps[i].SetValue(i < index);
            if (i == Lamps.Length - 1)
            {
                if (i < index)
                {
                    LastPointOn.Invoke();
                }
                else
                {
                    LastPointOff.Invoke();
                }
            }
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