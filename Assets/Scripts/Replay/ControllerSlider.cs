using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class ControllerSlider : MonoBehaviour
    {
        static Slider slider;

        static TextMeshProUGUI minText;
        static TextMeshProUGUI maxText;
        static TextMeshProUGUI currentTime;

        private void Awake()
        {
            minText = GetText(0);
            maxText = GetText(1);
            currentTime = GetText(2);

            slider = gameObject.GetComponent<Slider>();
            slider.onValueChanged.AddListener(ReplaySystem.SetTime);

            // - inner function
            TextMeshProUGUI GetText(int n)
            {
                return gameObject.transform.GetChild(n).gameObject.GetComponent<TextMeshProUGUI>();
            }
        }

        void Start()
        {
            UpdateContent();
            SetEvent(1);
        }

        static void UpdateContent()
        {
            minText.text = TxtUtil.Time(0.0f, false);
            maxText.text = TxtUtil.Time(ReplaySystem.EndTime, false);

            slider.maxValue = ReplaySystem.EndTime;
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                ReplaySystem.Updated += SetValue;
            }

            else
            {
                ReplaySystem.Updated -= SetValue;
            }
        }

        static void SetValue(object obj, float[] data)
        {
            slider.value = ReplaySystem.PastTime;

            currentTime.text = TxtUtil.Time(ReplaySystem.PastTime, true);
        }
    }
}

