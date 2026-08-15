using System;
using UnityEngine;

public class LampBlink : MonoBehaviour
{
    public bool On;
    public CanvasGroup Fade;
    public float Frequency;
    public float OffAlpha;
    public AudioSource Audio;

    public float Offset;

    public void Update()
    {
        if (On)
        {
            Audio.enabled = true;
            Fade.alpha = Mathf.Lerp(OffAlpha, 1, (Mathf.Sin(Frequency * Offset) + 1) / 2);
            Offset += Time.deltaTime;
        }
        else
        {
            Audio.enabled = false;
            Offset = 0;
            Fade.alpha = OffAlpha;
        }
    }

    public void SetValue(bool value)
    {
        On = value;
    }
}