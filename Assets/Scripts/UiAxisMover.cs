using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UiAxisMover : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public UnityEvent<float> OnUpdate;
    
    public float MinLimit;
    public float MaxLimit;
    public float NormalizedPosition = 0f;
    public bool AllowDragging = true;

    public RectTransform RectTransform;
    public Canvas ParentCanvas;

    public void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        ParentCanvas = GetComponentInParent<Canvas>();
        UpdateNormalizedPosition();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (AllowDragging == false || ParentCanvas == null) return;

        var delta = eventData.delta / ParentCanvas.scaleFactor;
        var pos = RectTransform.anchoredPosition;

        pos.y = Mathf.Clamp(pos.y + delta.y, MinLimit, MaxLimit);

        RectTransform.anchoredPosition = pos;
        UpdateNormalizedPosition();
    }

    public void SetPosition(float targetPosition)
    {
        var pos = RectTransform.anchoredPosition;
        pos.y = Mathf.Clamp(targetPosition, MinLimit, MaxLimit);
        RectTransform.anchoredPosition = pos;
        UpdateNormalizedPosition();
    }

    public void SetNormalizedPosition(float value)
    {
        NormalizedPosition = Mathf.Clamp01(value);
        var targetPos = Mathf.Lerp(MinLimit, MaxLimit, NormalizedPosition);
        var pos = RectTransform.anchoredPosition;
        pos.y = targetPos;
        RectTransform.anchoredPosition = pos;
    }

    public void UpdateNormalizedPosition()
    {
        NormalizedPosition = Mathf.InverseLerp(MinLimit, MaxLimit, RectTransform.anchoredPosition.y);
        OnUpdate.Invoke(NormalizedPosition);
    }
}