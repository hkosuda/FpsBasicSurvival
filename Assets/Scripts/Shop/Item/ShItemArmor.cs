using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemArmor : ShopItemButton
    {
        private void Awake()
        {
            Initialize(ShopItem.armor);
        }

        protected override string CalcCurrentValue()
        {
            currentValue = SV_Status.CurrentArmor;
            return currentValue.ToString("#,0");
        }

        protected override string CalcNextValue()
        {
            CalcCurrentValue();

            var nCart = SV_ShopItem.CartList[Item];
            nextValue = currentValue + nCart * increase;

            if (nextValue > ShItemMaxArmor.NextMaxArmor)
            {
                nextValue = SV_Status.CurrentMaxArmor;
            }

            return nextValue.ToString();
        }

        protected override bool CheckAddToCart()
        {
            if (!base.CheckAddToCart()) { return false; }

            CalcNextValue();

            if (nextValue >= ShItemMaxArmor.NextMaxArmor)
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
            SV_Status.SetArmor(nextValue);
        }
    }
}

