using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class WindowInputField : MonoBehaviour
    {
        Image image;

        private void Start()
        {
            image = gameObject.GetComponent<Image>();
            image.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
    }
}

