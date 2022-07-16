using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class ShopPanel_Message : MonoBehaviour
    {
        static TextMeshProUGUI messageText;

        private void Awake()
        {
            messageText = gameObject.GetComponent<TextMeshProUGUI>();
            messageText.text = "";
        }

        static public void UpdateDiscription(ShopItem item, string discription)
        {
            if (messageText == null) { return; }

            messageText.text = "�y" + ShopItems.ItemNames[item] + "�z\n";
            messageText.text += discription;
        }
    }
}

