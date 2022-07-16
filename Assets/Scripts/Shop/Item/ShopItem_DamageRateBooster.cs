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
        var discription = "�@�G�ɗ^����_���[�W�𑝉������܂��D�_���[�W�𑝉������邱�Ƃ́C�G�����j����܂łɂ����鎞�Ԃ�Z�k�����邾���łȂ��C�e��̐ߖ�ɂ��Ȃ���܂��D\n" +
            "�@�Ȃ��C�G�̗̑͂̓��E���h���Ƃɑ������Ă������߁C�p���I�ɂ��̃A�b�v�O���[�g���s�����Ƃ������߂��܂��D";

        return discription;
    }
}
