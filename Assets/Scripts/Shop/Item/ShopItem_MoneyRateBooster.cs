using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem_MoneyRateBooster : ShopItems
{
    private void Awake()
    {
        AwakeMethod(ShopItem.money_rate_booster,
            Ints.Item.shop_item_money_rate_booster_amount,
            Ints.Item.shop_item_money_rate_booster_cost_default,
            Ints.Item.shop_item_money_rate_booster_cost_increase);
    }

    protected override string GetCurrentValueString()
    {
        return SV_StatusAdmin.CurrentMoneyRate.ToString();
    }

    protected override string GetNextValueString()
    {
        return SV_ShopAdmin.NextMoneyRate.ToString();
    }

    protected override string GetDiscription()
    {
        var discription = "　獲得するマネーを増加させます．";

        return discription;
    }
}
