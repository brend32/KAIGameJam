using System;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    public bool On;
    public SmoothFade Fade;

    public void Start()
    {
        SetValue(On);
    }

    public void SetValue(bool on)
    {
        On = on;
        Fade.Alpha = on ? 1f : 0f;
    }
}