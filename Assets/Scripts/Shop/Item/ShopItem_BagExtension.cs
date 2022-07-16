using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem_BagExtension : ShopItems
{
    private void Awake()
    {
        AwakeMethod(ShopItem.bag_extension,
            Ints.Item.shop_item_bag_extension_amount,
            Ints.Item.shop_item_bag_extension_cost_default,
            Ints.Item.shop_item_bag_extension_cost_increase);
    }

    protected override string GetCurrentValueString()
    {
        return AkController.MaxAmmoInBag.ToString();
    }

    protected override string GetNextValueString()
    {
        return SV_ShopAdmin.NextMaxAmmoInBag.ToString();
    }

    protected override string GetDiscription()
    {
        var discription = "�@�g�тł���AKM�̒e��̐��𑝉������܂��D�e�򂪂����ƃN���A�����ɍ���ɂȂ�܂��D" +
            "�K�v�ɉ����āC���₵�Ă����悤�ɂ��܂��傤�D";

        return discription;
    }
}
