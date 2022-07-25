using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class WindowMain : MonoBehaviour
    {
        static public readonly float alpha = 0.7f;

        static readonly Color windowColor = new Color(0.0f, 0.0f, 0.0f, alpha);

        void Start()
        {
            var image = gameObject.GetComponent<Image>();
            image.color = windowColor;
        }
    }
}

