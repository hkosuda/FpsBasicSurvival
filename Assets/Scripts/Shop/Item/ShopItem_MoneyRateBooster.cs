using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShopItem_MoneyRateBooster : ShopItems
    {
        private void Awake()
        {
            AwakeMethod(ShopItem.money_rate_booster,
                Params.shop_money_rate_booster_amount,
                Params.shop_money_rate_booster_cost_default,
                Params.shop_money_rate_booster_cost_increase);
        }

        protected override string GetCurrentValueString()
        {
            return SV_StatusAdmin.CurrentMoneyRate.ToString();
        }

        protected override string GetNextValueString()
        {
            return SV_ShopAdmin.NextMoneyRate.ToString();
        }

        protected override string GetDescription()
        {
            var discription = "　獲得するマネーを増加させます．";

            return discription;
        }
    }
}

