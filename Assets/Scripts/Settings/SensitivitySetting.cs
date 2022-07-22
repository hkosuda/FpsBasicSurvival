using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class SensitivitySetting : MonoBehaviour
    {
        static Slider slider;
        static TMP_InputField inputField;

        private void Awake()
        {
            slider = gameObject.GetComponent<Slider>();
            inputField = gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_InputField>();

            slider.minValue = 0.1f;
            slider.maxValue = 10.0f;

            slider.onValueChanged.AddListener(UpdateInputField);
            inputField.onEndEdit.AddListener(UpdateSlider);

            slider.value = Params.mouse_sens;
            inputField.text = Params.mouse_sens.ToString("f2");
        }

        void UpdateInputField(float value)
        {
            ChageSensi(value);
            inputField.text = Params.mouse_sens.ToString("f2");
        }

        void UpdateSlider(string value)
        {
            if (float.TryParse(value, out var num))
            {
                ChageSensi(num);
                slider.value = Params.mouse_sens;
            }
        }

        void ChageSensi(float value)
        {
            Params.mouse_sens = value;
        }
    }
}

