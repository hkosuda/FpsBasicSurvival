using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class ShopPanel_RemainMoneyText : MonoBehaviour
    {
        TextMeshProUGUI moneyText;

        private void Awake()
        {
            moneyText = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                SV_ShopAdmin.CartUpdated += UpdateContent;
            }

            else
            {
                SV_ShopAdmin.CartUpdated -= UpdateContent;
            }
        }

        void UpdateContent(object obj, bool mute)
        {
            moneyText.text = "�c�� : $ " + SV_ShopAdmin.MoneyRemain.ToString("#,0");
        }
    }
}

