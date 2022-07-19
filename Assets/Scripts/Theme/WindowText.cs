using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class WindowText : MonoBehaviour
    {
        static readonly Color textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        void Start()
        {
            var text = gameObject.GetComponent<TextMeshProUGUI>();
            text.color = textColor;
        }
    }
}

