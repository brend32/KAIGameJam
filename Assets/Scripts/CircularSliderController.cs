using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CircularSliderController : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public float MinAngle = -90f;
    public float MaxAngle = 90f;
    public float MinValue = 0f;
    public float MaxValue = 999f;
    public float CurrentValue = 500f;
    public float NormalizedValue = 0.5f;
    public float DragSensitivity = 0.5f;

    public RectTransform KnobTransform;
    public Canvas ParentCanvas;
    public TextMeshProUGUI Text;

    public void Awake()
    {
        if (KnobTransform == null)
            KnobTransform = GetComponent<RectTransform>();

        if (ParentCanvas == null)
            ParentCanvas = GetComponentInParent<Canvas>();

        UpdateKnobRotation();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        float canvasScale = (ParentCanvas != null) ? ParentCanvas.scaleFactor : 1f;
        Vector2 delta = eventData.delta / canvasScale;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            KnobTransform,
            eventData.position,
            eventData.enterEventCamera,
            out Vector2 localPoint
        );

        float verticalMultiplier = (localPoint.x >= 0f) ? -1f : 1f;
        float angleDelta = (delta.x + (delta.y * verticalMultiplier)) * DragSensitivity;

        float currentAngle = Mathf.Lerp(MinAngle, MaxAngle, NormalizedValue);
        currentAngle = Mathf.Clamp(currentAngle + angleDelta, MinAngle, MaxAngle);

        NormalizedValue = Mathf.InverseLerp(MinAngle, MaxAngle, currentAngle);
        CurrentValue = Mathf.Lerp(MinValue, MaxValue, NormalizedValue);

        UpdateKnobRotation();
    }

    public void SetNormalizedValue(float value)
    {
        NormalizedValue = Mathf.Clamp01(value);
        CurrentValue = Mathf.Lerp(MinValue, MaxValue, NormalizedValue);
        UpdateKnobRotation();
    }

    public void SetValue(float value)
    {
        CurrentValue = Mathf.Clamp(value, MinValue, MaxValue);
        NormalizedValue = Mathf.InverseLerp(MinValue, MaxValue, CurrentValue);
        UpdateKnobRotation();
    }

    public void UpdateKnobRotation()
    {
        float currentAngle = Mathf.Lerp(MinAngle, MaxAngle, NormalizedValue);
        KnobTransform.localRotation = Quaternion.Euler(0f, 0f, -currentAngle);
        Text.text = Mathf.FloorToInt(CurrentValue).ToString();
    }
}