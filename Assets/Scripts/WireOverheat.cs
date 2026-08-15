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
    
    private SpriteRenderer wireRenderer;

    void Awake()
    {
        wireRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        bool isPressingSpace = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;

        if (isPressingSpace)
        {
            currentHeat += heatRate * Time.deltaTime;
        }
        else
        {
            currentHeat -= coolRate * Time.deltaTime;
        }

        currentHeat = Mathf.Clamp(currentHeat, 0f, maxHeat);

        UpdateWireColor();
    }

    private void UpdateWireColor()
    {
        float heatPercent = currentHeat / maxHeat;
        wireRenderer.color = heatGradient.Evaluate(heatPercent);
    }
}