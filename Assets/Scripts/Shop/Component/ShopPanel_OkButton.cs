using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class ShopPanel_OkButton : MonoBehaviour
    {
        void Start()
        {
            var button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(CancelAfterConfirmation);
        }

        void CancelAfterConfirmation()
        {
            Confirmation.BeginConfirmation("アップグレートを行い，次のラウンドに進みますか？", ShopPanel.DestroyShopPanel, null);
        }
    }
}

