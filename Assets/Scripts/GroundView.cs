using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GroundView : MonoBehaviour
    {
        public float Min;
        public float Max;
        public float Value;
        public DrillDevice DrillDevice;

        public RectTransform Transform;

        public void Start()
        {
            Transform = GetComponent<RectTransform>();
        }

        public void Update()
        {
            var pos = Transform.anchoredPosition;
            pos.y = Mathf.Lerp(Min, Max, Value);
            Transform.anchoredPosition = pos;

            Value = DrillDevice.NormalizedDepth;
        }
    }
}