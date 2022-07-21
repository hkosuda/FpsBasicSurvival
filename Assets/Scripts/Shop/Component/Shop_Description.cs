using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class Shop_Description : MonoBehaviour
    {
        static public TextMeshProUGUI text;

        void Start()
        {
            text = gameObject.GetComponent<TextMeshProUGUI>();
        }

        static public void ShowDescription(string name, string description)
        {
            var info = "Åy" + name + "Åz\n";
            info += description;

            text.text = info;
        }
    }
}

