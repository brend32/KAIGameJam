using System;
using UnityEngine;

public class EnvRadiationLink : MonoBehaviour
{
    public EnvTesterDevice Tester;
    public RadiationView Display;
    public float CheckTimer;

    public float Time;

    public void Update()
    {
        if (Time < 0)
        {
            Display.TargetValue = Tester.DisplayedRadiation / 1000f;
            Time = CheckTimer;
        }
        else
        {
            Time -= UnityEngine.Time.deltaTime;
        }
    }
}