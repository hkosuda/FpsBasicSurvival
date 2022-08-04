using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemMoneyRate : ShopItemButton
    {
        static readonly string extension = " [%]";

        private void Awake()
        {
            Initialize(ShopItem.money_rate,
                SvParams.GetInt(SvParam.shop_money_rate_amount),
                SvParams.GetInt(SvParam.shop_money_rate_cost_default),
                SvParams.GetInt(SvParam.shop_money_rate_cost_increase));
        }

        protected override string CalcCurrentValue()
        {
            currentValue = SV_Status.CurrentMoneyRate;
            return currentValue.ToString() + extension;
        }

        protected override string CalcNextValue()
        {
            CalcCurrentValue();

            var nCart = SV_ShopItem.CartList[Item];
            nextValue = currentValue + nCart * increase;

            return nextValue.ToString() + extension;
        }

        protected override string Description()
        {
            return "�l������}�l�[�̗ʂ𑝉������܂��D\n" +
                "�A�b�v�O���[�h�̉��i�͏㏸���Â��܂��D���̂��߁C�}�l�[�l�������㏸�����ăA�b�v�O���[�h���p���I�ɍw���ł���悤�ɂ��܂��傤�D";
        }

        protected override void Apply()
        {
            SV_Status.SetMoneyRate(nextValue);
        }
    }
}
