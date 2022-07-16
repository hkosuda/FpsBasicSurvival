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
        var discription = "�@���e���𑝉������܂��D�����̓G�Ɉ͂܂ꂽ�ۂɁC���̃A�b�v�O���[�g�����ɗ���������܂���D";

        return discription;
    }
}
