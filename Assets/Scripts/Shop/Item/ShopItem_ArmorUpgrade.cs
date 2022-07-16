using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem_ArmorUpgrade : ShopItems
{
    private void Awake()
    {
        AwakeMethod(ShopItem.armor_upgrade,
            Ints.Item.shop_item_armor_upgrade_amount,
            Ints.Item.shop_item_armor_upgrade_cost_default,
            Ints.Item.shop_item_armor_upgrade_cost_increase);
    }

    protected override string GetCurrentValueString()
    {
        return SV_StatusAdmin.CurrentMaxArmor.ToString();
    }

    protected override string GetNextValueString()
    {
        return SV_ShopAdmin.NextMaxArmor.ToString();
    }

    protected override string GetDiscription()
    {
        var discription = "　アーマーの耐久値の上限を増加させます．";

        return discription;
    }
}
