using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShopItem_ArmorRepairing : ShopItems
    {
        private void Awake()
        {
            AwakeMethod(ShopItem.armor_repairing,
                Params.shop_armor_repairing_amount,
                Params.shop_armor_repairing_cost_default,
                Params.shop_armor_repairing_cost_increase);
        }

        protected override bool CheckAddToCart()
        {
            if (SV_ShopAdmin.MoneyRemain < CurrentCost())
            {
                return false;
            }

            var currentArmor = SV_StatusAdmin.StatusList[SV_Status.armor];

            var nextArmor = currentArmor + amount * SV_ShopAdmin.CartList[item];
            var nextMaxArmor = SV_ShopAdmin.NextMaxArmor;

            if (nextArmor >= nextMaxArmor)
            {
                return false;
            }

            return true;
        }

        protected override string GetCurrentValueString()
        {
            return SV_StatusAdmin.StatusList[SV_Status.armor].ToString();
        }

        protected override string GetNextValueString()
        {
            return SV_ShopAdmin.NextArmor.ToString();
        }

        protected override string GetDescription()
        {
            var constant = Params.damage_reduction_const;

            var discription = "�@�A�[�}�[�̑ϋv�l���񕜂��܂��D\n" +
                "�@�A�[�}�[�̑ϋv�l���傫���قǎ󂯂�_���[�W�͏������Ȃ邽�߁C�ϋv�l���Ⴂ�ꍇ�͉񕜂��Ă������Ƃ������߂��܂��D\n" +
                "�i�v���C���[���󂯂�_���[�W�́C�^����ꂽ�_���[�W�� '" + constant.ToString() + " / �i�A�[�}�[�ϋv�l + " + constant.ToString() + "'�{" +
                "�����l�ƂȂ�܂��j";

            return discription;
        }
    }
}

