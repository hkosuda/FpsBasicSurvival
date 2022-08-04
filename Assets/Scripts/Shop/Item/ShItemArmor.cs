using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemArmor : ShopItemButton
    {
        private void Awake()
        {
            Initialize(ShopItem.armor,
                SvParams.GetInt(SvParam.shop_armor_amount),
                SvParams.GetInt(SvParam.shop_armor_cost_default),
                SvParams.GetInt(SvParam.shop_armor_cost_increase));
        }

        protected override string CalcCurrentValue()
        {
            currentValue = SV_Status.CurrentArmor;
            return currentValue.ToString("#,0");
        }

        protected override string CalcNextValue()
        {
            CalcCurrentValue();

            var nCart = SV_ShopItem.CartList[Item];
            nextValue = currentValue + nCart * increase;

            var max = ShItemMaxArmor.NextMaxArmor;

            if (nextValue > max)
            {
                nextValue = max;

                var nlim = NCartLimit(nCart, max);
                if (nCart > nlim) { SV_ShopItem.CartList[Item] = nlim; }
            }

            return nextValue.ToString();

            
        }

        protected override bool CheckAddToCart()
        {
            if (!base.CheckAddToCart()) { return false; }

            CalcNextValue();

            if (nextValue >= ShItemMaxArmor.NextMaxArmor)
            {
                return false;
            }

            return true;
        }

        protected override string Description()
        {
            return "�A�[�}�[�̑ϋv�l���񕜂��܂��D\n" +
                "�v���C���[���󂯂�_���[�W�́C�A�[�}�[�̑ϋv�l�������قǏ������Ȃ�܂��D��̓I�ɁC��_���[�W�ɑ΂���HP�����ʂ͈ȉ��̎��Ōv�Z����܂��D\n" +
                "(HP������) = (��_���[�W) x 1000 / {1000 + (�A�[�}�[�ϋv�l)}\n" +
                "�Ȃ��C��_���[�W�ɑ΂���A�[�}�[�ϋv�l�̌����ʂ͏�ɔ�_���[�W��50%�ł��D";
        }

        protected override void Apply()
        {
            SV_Status.SetArmor(nextValue);
        }
    }
}

