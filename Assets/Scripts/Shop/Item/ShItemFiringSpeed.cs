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
            return "射撃間隔を短縮させます．\n" +
                "射撃間隔を短縮させることで，敵を撃破するまでの時間を短縮することができます．しかし，消費弾数が増加しがちになるため，携帯弾数や装弾数に気を付けましょう．";
        }

        protected override void Apply()
        {
            SV_Status.SetFiringSpeedRate(nextValue);

            var interval = Const.ak_firing_interval / SV_Status.FiringSpeed();
            AK_Potential.UpdateShootingInterval(interval);
        }
    }
}
