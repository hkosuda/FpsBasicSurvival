using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemDamageRate : ShopItemButton
    {
        static readonly string extension = " [%]";

        private void Awake()
        {
            Initialize(ShopItem.damage_rate);
        }

        protected override string CalcCurrentValue()
        {
            currentValue = SV_Status.CurrentDamageRate;
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
            return "与ダメージ率を増加させます．";
        }

        protected override void Apply()
        {
            CalcNextValue();
            SV_Status.CurrentDamageRate = nextValue;
        }
    }
}

