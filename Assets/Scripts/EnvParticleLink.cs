using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnvParticleLink : MonoBehaviour
    {
        public EnvTesterDevice Tester;
        public TextMeshProUGUI Particles;
        public Lamp Lamp;
        public int Count;
        public float TimerHold;

        public float Timer;

        public void Start()
        {
            Particles.text = Count.ToString();
        }

        public void Update()
        {
            if (Timer < 0)
            {
                Lamp.SetValue(false);
                var count = Mathf.FloorToInt(Tester.ParticlesCollected);
                if (count != Count)
                {
                    Count = count;
                    Blink();
                }
            }
            else
            {
                Timer -= Time.deltaTime;
            }
        }

        public void Blink()
        {
            Timer = TimerHold;
            Particles.text = Count.ToString();
            Lamp.SetValue(true);
        }
    }
}