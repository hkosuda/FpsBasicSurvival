using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class WindowButton : MonoBehaviour
    {
        static readonly Color textColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        static readonly Color mainColor = new Color(0.4f, 0.4f, 0.4f, 1.0f);

        Image buttonImage;
        TextMeshProUGUI text;

        void Start()
        {
            buttonImage = gameObject.GetComponent<Image>();
            text = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            buttonImage.color = mainColor;
            text.color = textColor;
        }
    }
}

