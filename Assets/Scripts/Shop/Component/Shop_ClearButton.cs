using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class Shop_ClearButton : MonoBehaviour
    {
        void Start()
        {
            var button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(CancelAfterConfirmation);
        }

        void CancelAfterConfirmation()
        {
            Confirmation.BeginConfirmation("•ÏX‚ğ‚·‚×‚Ä”jŠü‚µ‚Ü‚·‚©H", SV_ShopItem.ClearCart, null);
        }
    }
}

