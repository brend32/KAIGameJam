using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteToUIConverter : MonoBehaviour
{
    [Header("Target Field")]
    [Tooltip("Drag the root GameObject (or single sprite) here to convert it and all its children.")]
    [SerializeField] private GameObject targetObject;

    [Header("Settings")]
    [Tooltip("Target Canvas where the UI elements will be instantiated.")]
    [SerializeField] private Canvas targetCanvas;

    [Tooltip("Main Camera used to calculate screen positions.")]
    [SerializeField] private Camera mainCamera;

    [Tooltip("If true, destroys the original sprite hierarchy after converting.")]
    [SerializeField] private bool destroyOriginals = false;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    /// <summary>
    /// Right-click the component header in the Inspector to convert the tree.
    /// </summary>
    [ContextMenu("Convert Target Tree")]
    public void ConvertTargetFromInspector()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("No Target Object assigned in the Inspector field.");
            return;
        }

        ConvertHierarchyToUI(targetObject.transform);
    }

    /// <summary>
    /// Traverses a transform tree recursively and builds a matching UI RectTransform hierarchy under the Canvas.
    /// </summary>
    public GameObject ConvertHierarchyToUI(Transform rootTransform)
    {
        if (rootTransform == null) return null;

        if (targetCanvas == null)
        {
            targetCanvas = FindAnyObjectByType<Canvas>();
            if (targetCanvas == null)
            {
                Debug.LogError("No Canvas found in the scene to place UI elements under.");
                return null;
            }
        }

        Camera cam = (mainCamera != null) ? mainCamera : Camera.main;
        RectTransform canvasRect = targetCanvas.GetComponent<RectTransform>();

        // Dictionary to map original Transforms to created UI RectTransforms
        Dictionary<Transform, RectTransform> transformMap = new Dictionary<Transform, RectTransform>();

        // 1. Traverse and create UI objects recursively
        GameObject uiRoot = ConvertNodeRecursive(rootTransform, targetCanvas.transform, transformMap, cam, canvasRect);

        // 2. Destroy original tree if configured
        if (destroyOriginals && uiRoot != null)
        {
            if (Application.isPlaying)
                Destroy(rootTransform.gameObject);
            else
                DestroyImmediate(rootTransform.gameObject);
        }

        return uiRoot;
    }

    private GameObject ConvertNodeRecursive(
        Transform current,
        Transform uiParent,
        Dictionary<Transform, RectTransform> transformMap,
        Camera cam,
        RectTransform canvasRect)
    {
        // Create new UI GameObject
        GameObject uiObj = new GameObject(current.name + "_UI", typeof(RectTransform));
        RectTransform uiRect = uiObj.GetComponent<RectTransform>();
        uiRect.SetParent(uiParent, false);

        transformMap[current] = uiRect;

        // Check if current node has a SpriteRenderer
        SpriteRenderer sr = current.GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            Image img = uiObj.AddComponent<Image>();
            img.sprite = sr.sprite;
            img.color = sr.color;
            img.raycastTarget = false;

            // Align pivot to sprite settings
            Vector2 pivot = new Vector2(
                sr.sprite.pivot.x / sr.sprite.rect.width,
                sr.sprite.pivot.y / sr.sprite.rect.height
            );
            uiRect.pivot = pivot;

            // Position and size based on screen dimensions
            Vector3 worldPos = current.position;
            Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                uiParent as RectTransform ?? canvasRect,
                screenPos,
                targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam,
                out Vector2 localCanvasPos
            );

            uiRect.anchoredPosition = localCanvasPos;

            // Calculate dimensions
            Vector3 lossyScale = current.lossyScale;
            float width = (sr.sprite.rect.width / sr.sprite.pixelsPerUnit) * lossyScale.x;
            float height = (sr.sprite.rect.height / sr.sprite.pixelsPerUnit) * lossyScale.y;

            Vector3 corner1 = cam.WorldToScreenPoint(worldPos);
            Vector3 corner2 = cam.WorldToScreenPoint(worldPos + new Vector3(width, height, 0));
            Vector2 screenSize = new Vector2(Mathf.Abs(corner2.x - corner1.x), Mathf.Abs(corner2.y - corner1.y));

            uiRect.sizeDelta = screenSize / targetCanvas.scaleFactor;
        }
        else
        {
            // Empty container / non-sprite node: match screen position
            Vector3 screenPos = cam.WorldToScreenPoint(current.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                uiParent as RectTransform ?? canvasRect,
                screenPos,
                targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam,
                out Vector2 localCanvasPos
            );

            uiRect.anchoredPosition = localCanvasPos;
            uiRect.sizeDelta = Vector2.zero;
        }

        // Maintain relative rotation and scale
        uiRect.localRotation = current.localRotation;

        // Traverse all child transforms recursively
        for (int i = current.childCount - 1; i >= 0; i--)
        {
            var  child = current.GetChild(i);
            ConvertNodeRecursive(child, uiRect, transformMap, cam, canvasRect);
        }
        
        return uiObj;
    }
}