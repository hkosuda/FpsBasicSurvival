using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShopItem_BagExtension : ShopItems
    {
        private void Awake()
        {
            AwakeMethod(ShopItem.bag_extension,
                Params.shop_bag_extension_amount,
                Params.shop_bag_extension_cost_default,
                Params.shop_bag_extension_cost_increase);
        }

        protected override string GetCurrentValueString()
        {
            return "";
            //return AkController.MaxAmmoInBag.ToString();
        }

        protected override string GetNextValueString()
        {
            return SV_ShopAdmin.NextMaxAmmoInBag.ToString();
        }

        protected override string GetDescription()
        {
            var discription = "　携帯できるAKMの弾薬の数を増加させます．弾薬がきれるとクリアが非常に困難になります．" +
                "必要に応じて，増やしておくようにしましょう．";

            return discription;
        }
    }
}

