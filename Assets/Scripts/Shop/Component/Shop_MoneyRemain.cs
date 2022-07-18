using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class Shop_MoneyRemain : MonoBehaviour
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
                SV_ShopItem.TotalCostCalcEnd += UpdateContent;
            }

            else
            {
                SV_ShopItem.TotalCostCalcEnd -= UpdateContent;
            }
        }

        void UpdateContent(object obj, bool mute)
        {
            moneyText.text = "Žc‚è : $ " + SV_ShopItem.MoneyRemain.ToString("#,0");
        }
    }
}

