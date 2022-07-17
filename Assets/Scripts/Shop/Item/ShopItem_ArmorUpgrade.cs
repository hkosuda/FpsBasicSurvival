using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShopItem_ArmorUpgrade : ShopItems
    {
        private void Awake()
        {
            AwakeMethod(ShopItem.armor_upgrade,
                Params.shop_armor_upgrade_amount,
                Params.shop_armor_upgrade_cost_default,
                Params.shop_armor_upgrade_cost_increase);
        }

        protected override string GetCurrentValueString()
        {
            return SV_Status.CurrentMaxArmor.ToString();
        }

        protected override string GetNextValueString()
        {
            return SV_Shop.NextMaxArmor.ToString();
        }

        protected override string GetDescription()
        {
            var discription = "　アーマーの耐久値の上限を増加させます．";

            return discription;
        }
    }
}