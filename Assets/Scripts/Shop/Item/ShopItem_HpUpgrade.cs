using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShopItem_HpUpgrade : ShopItems
    {
        private void Awake()
        {
            AwakeMethod(ShopItem.hp_upgrade,
                Params.shop_hp_upgrade_amount,
                Params.shop_hp_upgrade_cost_default,
                Params.shop_hp_upgrade_cost_increase);
        }

        protected override string GetCurrentValueString()
        {
            return SV_StatusAdmin.CurrentMaxHP.ToString();
        }

        protected override string GetNextValueString()
        {
            return SV_ShopAdmin.NextMaxHP.ToString();
        }

        protected override string GetDescription()
        {
            var discription = "体力の上限を増加させます．";

            return discription;
        }
    }
}

