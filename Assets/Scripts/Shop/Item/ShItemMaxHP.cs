using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemMaxHP : ShopItemButton
    {
        static public int NextMaxHP { get; private set; }

        private void Awake()
        {
            Initialize(ShopItem.max_hp,
                SvParams.GetInt(SvParam.shop_max_hp_amount),
                SvParams.GetInt(SvParam.shop_max_hp_cost_default),
                SvParams.GetInt(SvParam.shop_max_hp_cost_increase));
        }

        protected override string CalcCurrentValue()
        {
            currentValue = SV_Status.CurrentMaxHP;
            return currentValue.ToString();
        }

        protected override string CalcNextValue()
        {
            CalcCurrentValue();

            var nCart = SV_ShopItem.CartList[Item];
            nextValue = currentValue + nCart * increase;

            NextMaxHP = nextValue;
            return nextValue.ToString();
        }

        protected override string Description()
        {
            return "�̗͂̍ő�l�𑝉������܂��D\n" +
                "���E���h���i�ނ��ƂɓG��1�����܂��܂��傫���Ȃ��Ă����܂��D" +
                "�̗͂������l�̂܂܂��Ƃ����ɃQ�[���I�[�o�[�ƂȂ��Ă��܂����߁C�A�b�v�O���[�g��ϋɓI�ɍs���܂��傤�D";
        }

        protected override void Apply()
        {
            SV_Status.SetMaxHP(nextValue);
        }
    }
}

