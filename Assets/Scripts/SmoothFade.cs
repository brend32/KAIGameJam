using System;
using UnityEngine;

public class SmoothFade : MonoBehaviour
{
    public CanvasGroup Group;
    public float Alpha;
    public float Decay = 6f;

    public void Update()
    {
        Group.alpha = ExpDecay(Group.alpha, Alpha, Decay, Time.deltaTime);
    }
    
    public static float ExpDecay(float a, float b, float decay, float deltaTime)
    {
        return b + (a - b) * Mathf.Exp(-decay * deltaTime);
    }
}