using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemArmor : ShopItemButton
    {
        private void Awake()
        {
            Initialize(ShopItem.armor,
                SvParams.GetInt(SvParam.shop_armor_amount),
                SvParams.GetInt(SvParam.shop_armor_cost_default),
                SvParams.GetInt(SvParam.shop_armor_cost_increase));
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

            var max = ShItemMaxArmor.NextMaxArmor;

            if (nextValue > max)
            {
                nextValue = max;

                var nlim = NCartLimit(nCart, max);
                if (nCart > nlim) { SV_ShopItem.CartList[Item] = nlim; }
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
            SV_Status.SetArmor(nextValue);
        }
    }
}

