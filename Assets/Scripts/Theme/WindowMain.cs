using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class WindowMain : MonoBehaviour
    {
        static readonly Color windowColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);

        void Start()
        {
            var image = gameObject.GetComponent<Image>();
            image.color = windowColor;
        }
    }
}

