using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class ControllerInputField : MonoBehaviour
    {
        static TMP_InputField inputField;

        private void Awake()
        {
            inputField = gameObject.GetComponent<TMP_InputField>();
            inputField.onEndEdit.AddListener(TryChangeSpeed);
        }

        private void Start()
        {
            UpdateText();
        }

        static void TryChangeSpeed(string value)
        {
            if (float.TryParse(value, out var speed))
            {
                ReplaySystem.SetSpeed(speed);
            }

            UpdateText();
        }

        static void UpdateText()
        {
            inputField.text = ReplaySystem.Speed.ToString("F2");
        }
    }
}

