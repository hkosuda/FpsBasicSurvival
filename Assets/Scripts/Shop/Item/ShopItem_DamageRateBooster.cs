using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem_DamageRateBooster : ShopItems
{
    private void Awake()
    {
        AwakeMethod(ShopItem.damage_rate_booster,
            Ints.Item.shop_item_damage_rate_booster_amount,
            Ints.Item.shop_item_damage_rate_booster_cost_default,
            Ints.Item.shop_item_damage_rate_booster_cost_increase);
    }

    protected override string GetCurrentValueString()
    {
        return SV_StatusAdmin.CurrentDamageRate.ToString();
    }

    protected override string GetNextValueString()
    {
        return SV_ShopAdmin.NextDamageRate.ToString();
    }

    protected override string GetDiscription()
    {
        var discription = "　敵に与えるダメージを増加させます．ダメージを増加させることは，敵を撃破するまでにかかる時間を短縮させるだけでなく，弾薬の節約にもつながります．\n" +
            "　なお，敵の体力はラウンドごとに増加していくため，継続的にこのアップグレートを行うことをお勧めします．";

        return discription;
    }
}
