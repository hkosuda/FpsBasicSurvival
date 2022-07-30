using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class WindowChatMain : MonoBehaviour
    {
        static public readonly float alpha = 0.3f;

        static readonly Color windowColor = new Color(0.0f, 0.0f, 0.0f, alpha);

        void Start()
        {
            var image = gameObject.GetComponent<Image>();
            image.color = windowColor;
        }
    }
}
