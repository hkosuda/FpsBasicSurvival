using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem_MagExtension : ShopItems
{
    private void Awake()
    {
        AwakeMethod(ShopItem.mag_extension,
            Ints.Item.shop_item_mag_extension_amount,
            Ints.Item.shop_item_mag_extension_cost_default,
            Ints.Item.shop_item_mag_extension_cost_increase);
    }

    protected override string GetCurrentValueString()
    {
        return AkController.MaxAmmoInMag.ToString();
    }

    protected override string GetNextValueString()
    {
        return SV_ShopAdmin.NextMaxAmmoInMag.ToString();
    }

    protected override string GetDiscription()
    {
        var discription = "　装弾数を増加させます．多数の敵に囲まれた際に，このアップグレートが役に立つかもしれません．";

        return discription;
    }
}
