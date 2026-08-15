using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class WireOverheat : MonoBehaviour
{
    [SerializeField] private float currentHeat = 0f;
    [SerializeField] private float maxHeat = 100f;
    [SerializeField] private float heatRate = 30f;
    [SerializeField] private float coolRate = 15f;

    [SerializeField] private Gradient heatGradient;
    [SerializeField] private float minVolume = 0.1f;
    [SerializeField] private float maxVolume = 1.0f;
    [SerializeField] private float criticalThreshold = 90f;

    private SpriteRenderer wireRenderer;

    void Awake()
    {
        wireRenderer = GetComponent<SpriteRenderer>();
    }

    private void UpdateWireSound()
    {
        if (AudioManager.Instance == null) return;

        AudioSource wireAudio = AudioManager.Instance.wireLoopSource;
        if (wireAudio == null) return;

        if (currentHeat >= criticalThreshold)
        {
            float volumePercent = (currentHeat - criticalThreshold) / (maxHeat - criticalThreshold);
            wireAudio.volume = Mathf.Lerp(minVolume, maxVolume, volumePercent);

            if (!wireAudio.isPlaying) wireAudio.Play();
        }
        else
        {
            wireAudio.volume = 0f;
            if (wireAudio.isPlaying) wireAudio.Stop();
        }
    }

    private void UpdateWireColor()
    {
        float heatPercent = currentHeat / maxHeat;
        wireRenderer.color = heatGradient.Evaluate(heatPercent);
    }
}