using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class SVUI_Time : MonoBehaviour
    {
        static TextMeshProUGUI text;

        private void Awake()
        {
            text = gameObject.GetComponent<TextMeshProUGUI>();
        }

        static public void UpdateText(string timeText)
        {
            text.text = timeText; 
        }
    }
}

