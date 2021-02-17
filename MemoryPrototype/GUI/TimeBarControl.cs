using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryPrototype.Gui
{
    public class TimeBarControl : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        public void SetMaxTime(int time)
        {
            slider.maxValue = time;
            slider.value = time;
        }

        public void SetTime(int time)
        {
            slider.value = time;
        }

        public void SetTimeFloat(float time)
        {
            slider.value = time;
        }

        public void SetMaxTimePercent(float time)
        {
            slider.maxValue = time;
        }
    }
}

