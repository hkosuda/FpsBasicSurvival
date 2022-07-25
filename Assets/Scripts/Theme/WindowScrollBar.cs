using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class WindowScrollBar : MonoBehaviour
    {
        static readonly Color backColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        static readonly Color handleColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);

        void Start()
        {
            var back = gameObject.GetComponent<Image>();
            var handle = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();

            back.color = backColor;
            handle.color = handleColor;
        }
    }
}

