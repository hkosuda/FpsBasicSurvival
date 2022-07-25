using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class WindowHeader : MonoBehaviour
    {
        static public readonly float alpha = 0.7f;

        static readonly Color textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        static readonly Color mainColor = new Color(0.0f, 0.0f, 1.0f, alpha);

        Image headerImage;
        TextMeshProUGUI text;

        void Start()
        {
            headerImage = gameObject.GetComponent<Image>();
            text = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            headerImage.color = mainColor;
            text.color = textColor;
        }
    }
}

