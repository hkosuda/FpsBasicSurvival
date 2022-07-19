using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class WindowFrame : MonoBehaviour
    {
        static readonly Color frameColor = new Color(0.7f, 0.7f, 0.7f, 1.0f);

        void Start()
        {
            var nChild = gameObject.transform.childCount;

            for(var n = 0; n < nChild; n++)
            {
                var image = gameObject.transform.GetChild(n).gameObject.GetComponent<Image>();
                image.color = frameColor;
            }
            
        }
    }
}

