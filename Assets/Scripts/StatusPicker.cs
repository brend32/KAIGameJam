using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class StatusPicker : MonoBehaviour
    {
        public float[] Threshold;
        public Color[] BackColor;
        public Color[] TextColor;
        public Image Back;
        public TextMeshProUGUI Text;
        public string[] TextContent;

        public float Value;

        public void Update()
        {
            for (int i = Threshold.Length - 1; i >= 0; i--)
            {
                var threshold = Threshold[i];
                if (threshold < Value || i == 0)
                {
                    Text.text = TextContent[i];
                    Back.color = BackColor[i];
                    Text.color = TextColor[i];
                    break;
                }
            }
        }

        private void OnValidate()
        {
            Update();
        }
    }
}