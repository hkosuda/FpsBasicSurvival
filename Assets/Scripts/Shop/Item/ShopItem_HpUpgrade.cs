using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem_HpUpgrade : ShopItems
{
    private void Awake()
    {
        AwakeMethod(ShopItem.hp_upgrade,
            Ints.Item.shop_item_hp_upgrade_amount,
            Ints.Item.shop_item_hp_upgrade_cost_default,
            Ints.Item.shop_item_hp_upgrade_cost_increase);
    }

    protected override string GetCurrentValueString()
    {
        return SV_StatusAdmin.CurrentMaxHP.ToString();
    }

    protected override string GetNextValueString()
    {
        return SV_ShopAdmin.NextMaxHP.ToString();
    }

    protected override string GetDiscription()
    {
        var discription = "ëÃóÕÇÃè„å¿Çëùâ¡Ç≥ÇπÇ‹Ç∑ÅD";

        return discription;
    }
}
