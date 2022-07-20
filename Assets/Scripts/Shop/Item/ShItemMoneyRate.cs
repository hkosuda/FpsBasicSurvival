using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemMoneyRate : ShopItemButton
    {
        static readonly string extension = " [%]";

        private void Awake()
        {
            Initialize(ShopItem.money_rate);
        }

        protected override string CalcCurrentValue()
        {
            currentValue = SV_Status.CurrentMoneyRate;
            return currentValue.ToString() + extension;
        }

        protected override string CalcNextValue()
        {
            CalcCurrentValue();

            var nCart = SV_ShopItem.CartList[Item];
            nextValue = currentValue + nCart * increase;

            return nextValue.ToString() + extension;
        }

        protected override string Description()
        {
            return "獲得するマネーの量を増加させます．";
        }

        protected override void Apply()
        {
            CalcNextValue();
            SV_Status.SetMoneyRate(nextValue);
        }
    }
}
