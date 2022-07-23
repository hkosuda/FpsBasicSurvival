using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemMovingSpeed : ShopItemButton
    {
        static readonly string extension = " [%]";

        private void Awake()
        {
            Initialize(ShopItem.moving_speed,
                SvParams.GetInt(SvParam.shop_moving_speed_amount),
                SvParams.GetInt(SvParam.shop_moving_speed_cost_default),
                SvParams.GetInt(SvParam.shop_moving_speed_cost_increase));
        }

        protected override string CalcCurrentValue()
        {
            currentValue = SV_Status.MovingSpeedRate;
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
            return "移動スピードを上昇させます．";
        }

        protected override void Apply()
        {
            SV_Status.SetMovingSpeedRate(nextValue);
        }
    }
}
