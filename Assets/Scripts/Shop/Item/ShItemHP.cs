using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemHP : ShopItemButton
    {
        private void Awake()
        {
            Initialize(ShopItem.hp);
        }

        protected override string CalcCurrentValue()
        {
            currentValue = SV_Status.StatusList[Status.hp];
            return currentValue.ToString("#,0");
        }

        protected override string CalcNextValue()
        {
            currentValue = SV_Status.StatusList[Status.hp];

            var nCart = SV_ShopItem.CartList[Item];
            nextValue = currentValue + nCart * increase;

            if (nextValue > ShItemMaxHP.NextMaxHP)
            {
                nextValue = SV_Status.CurrentMaxHP;
            }

            return nextValue.ToString();
        }

        protected override bool CheckAddToCart()
        {
            if (!base.CheckAddToCart()) { return false; }

            CalcNextValue();

            if (nextValue >= ShItemMaxHP.NextMaxHP)
            {
                return false;
            }

            return true;
        }

        protected override string Description()
        {
            return "‘Ì—Í‚ğ‰ñ•œ‚µ‚Ü‚·D";
        }

        protected override void Apply()
        {
            CalcNextValue();
            SV_Status.StatusList[Status.hp] = nextValue;
        }
    }
}

