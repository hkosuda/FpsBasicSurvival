using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShopItem_MagExtension : ShopItems
    {
        private void Awake()
        {
            AwakeMethod(ShopItem.mag_extension,
                Params.shop_mag_extension_amount,
                Params.shop_mag_extension_cost_default,
                Params.shop_mag_extension_cost_increase);
        }

        protected override string GetCurrentValueString()
        {
            return "";
            //return AkController.MaxAmmoInMag.ToString();
        }

        protected override string GetNextValueString()
        {
            return SV_Shop.NextMaxAmmoInMag.ToString();
        }

        protected override string GetDescription()
        {
            var discription = "　装弾数を増加させます．多数の敵に囲まれた際に，このアップグレートが役に立つかもしれません．";

            return discription;
        }
    }
}

