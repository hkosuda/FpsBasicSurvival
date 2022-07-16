using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShopItem_HpHealing : ShopItems
    {
        private void Awake()
        {
            AwakeMethod(ShopItem.hp_healing,
                Params.shop_hp_healing_amount,
                Params.shop_hp_healing_cost_default,
                Params.shop_hp_healing_cost_increase);
        }

        protected override bool CheckAddToCart()
        {
            if (SV_ShopAdmin.MoneyRemain < CurrentCost())
            {
                return false;
            }

            var currentHP = SV_StatusAdmin.StatusList[SV_Status.hp];

            var nextHP = currentHP + amount * SV_ShopAdmin.CartList[item];
            var nextMaxHP = SV_ShopAdmin.NextMaxHP;

            if (nextHP >= nextMaxHP)
            {
                return false;
            }

            return true;
        }

        protected override string GetCurrentValueString()
        {
            return SV_StatusAdmin.StatusList[SV_Status.hp].ToString();
        }

        protected override string GetNextValueString()
        {
            return SV_ShopAdmin.NextHP.ToString();
        }

        protected override string GetDescription()
        {
            var discription = "Å@ëÃóÕÇâÒïúÇµÇ‹Ç∑ÅD";

            return discription;
        }
    }
}

