using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemFiringSpeed : ShopItemButton
    {
        static readonly string extension = " [%]";

        private void Awake()
        {
            Initialize(ShopItem.firing_speed,
                SvParams.GetInt(SvParam.shop_firing_speed_amount),
                SvParams.GetInt(SvParam.shop_firing_speed_cost_default),
                SvParams.GetInt(SvParam.shop_firing_speed_cost_increase));
        }

        protected override string CalcCurrentValue()
        {
            currentValue = SV_Status.FiringSpeedRate;
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
            return "�ˌ��Ԋu��Z�k�����܂��D\n" +
                "�ˌ��Ԋu��Z�k�����邱�ƂŁC�G�����j����܂ł̎��Ԃ�Z�k���邱�Ƃ��ł��܂��D�������C����e���������������ɂȂ邽�߁C�g�ђe���⑕�e���ɋC��t���܂��傤�D";
        }

        protected override void Apply()
        {
            SV_Status.SetFiringSpeedRate(nextValue);

            var interval = Const.ak_firing_interval / SV_Status.FiringSpeed();
            AK_Potential.UpdateShootingInterval(interval);
        }
    }
}
