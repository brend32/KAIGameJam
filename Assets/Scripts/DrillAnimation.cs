using UnityEngine;
using UnityEngine.UI;

public class DrillAnimation : MonoBehaviour
{
    public Sprite[] AnimationFrames;
    public float BaseSpriteFps = 10f;
    public float BaseWiggleFrequency = 5f;
    public float WiggleAmplitude = 0.5f;
    public float SpeedParameter = 1f;

    public Image Image;
    public Vector3 InitialLocalPosition;
    public float WigglePhase;
    public float FrameTimer;
    public int CurrentFrameIndex;
    public float Multiplier = 1;
    public DrillDevice Tester;

    public void Awake()
    {
        InitialLocalPosition = transform.localPosition;
    }

    public void Update()
    {
        SpeedParameter = Tester.Speed * Multiplier;
        float speed = Mathf.Max(0f, SpeedParameter);

        UpdateSpriteAnimation(speed);
        UpdateWigglePosition(speed);
    }

    public void UpdateSpriteAnimation(float speed)
    {
        if (AnimationFrames == null || AnimationFrames.Length == 0) return;

        float effectiveFps = BaseSpriteFps * speed;
        if (effectiveFps <= 0f) return;

        FrameTimer += Time.deltaTime * effectiveFps;
        if (FrameTimer >= 1f)
        {
            int framesToAdvance = Mathf.FloorToInt(FrameTimer);
            FrameTimer -= framesToAdvance;
            CurrentFrameIndex = (CurrentFrameIndex + framesToAdvance) % AnimationFrames.Length;
            Image.sprite = AnimationFrames[CurrentFrameIndex];
        }
    }

    public void UpdateWigglePosition(float speed)
    {
        float currentFrequency = BaseWiggleFrequency * speed;
        WigglePhase += Time.deltaTime * currentFrequency;

        float wiggleValue = TriangleWave(WigglePhase) * WiggleAmplitude;
        transform.localPosition = InitialLocalPosition + new Vector3(0f, wiggleValue, 0f);
    }

    public float TriangleWave(float t)
    {
        return Mathf.PingPong(t * 2f, 2f) - 1f;
    }

    public void SetSpeedParameter(float speed)
    {
        SpeedParameter = Mathf.Max(0f, speed);
    }
}