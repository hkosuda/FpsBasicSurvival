using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class ControllerKey : MonoBehaviour
    {
        public bool Value { get; private set; }

        List<Image> lines;
        TextMeshProUGUI text;

        private void Awake()
        {
            lines = new List<Image>()
            {
                GetImage(0), GetImage(1), GetImage(2), GetImage(3),
            };

            text = gameObject.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();

            // - inner function
            Image GetImage(int n)
            {
                return gameObject.transform.GetChild(n).gameObject.GetComponent<Image>();
            }
        }

        public void SetValue(bool value)
        {
            Value = value;
            UpdateColor();
        }

        void UpdateColor()
        {
            Color color;

            if (Value)
            {
                color = new Color(0.0f, 1.0f, 0.0f);
            }

            else
            {
                color = new Color(1.0f, 1.0f, 1.0f);
            }

            foreach(var line in lines)
            {
                line.color = color;
            }

            text.color = color;
        }
    }
}

