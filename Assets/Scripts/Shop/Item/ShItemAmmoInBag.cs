using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemAmmoInBag : ShopItemButton
    {
        private void Awake()
        {
            Initialize(ShopItem.ammo_in_bag, 
                SvParams.GetInt(SvParam.shop_bag_extension_amount), 
                SvParams.GetInt(SvParam.shop_bag_extension_cost_default), 
                SvParams.GetInt(SvParam.shop_bag_extension_cost_increase));
        }

        protected override string CalcCurrentValue()
        {
            currentValue = AK_Availability.MaxAmmoInBag;
            return currentValue.ToString();
        }

        protected override string CalcNextValue()
        {
            CalcCurrentValue();

            var nCart = SV_ShopItem.CartList[Item];
            nextValue = currentValue + nCart * increase;

            return nextValue.ToString();
        }

        protected override string Description()
        {
            return "ågë—íeêîÇëùâ¡Ç≥ÇπÇ‹Ç∑ÅD";
        }

        protected override void Apply()
        {
            AK_Availability.MaxAmmoInBag = nextValue;
            AK_Availability.AmmoInBag = AK_Availability.MaxAmmoInBag;
        }
    }
}