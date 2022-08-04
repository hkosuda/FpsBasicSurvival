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
            return "ËŒ‚ŠÔŠu‚ğ’Zk‚³‚¹‚Ü‚·D\n" +
                "ËŒ‚ŠÔŠu‚ğ’Zk‚³‚¹‚é‚±‚Æ‚ÅC“G‚ğŒ‚”j‚·‚é‚Ü‚Å‚ÌŠÔ‚ğ’Zk‚·‚é‚±‚Æ‚ª‚Å‚«‚Ü‚·D‚µ‚©‚µCÁ”ï’e”‚ª‘‰Á‚µ‚ª‚¿‚É‚È‚é‚½‚ßCŒg‘Ñ’e”‚â‘•’e”‚É‹C‚ğ•t‚¯‚Ü‚µ‚å‚¤D";
        }

        protected override void Apply()
        {
            SV_Status.SetFiringSpeedRate(nextValue);

            var interval = Const.ak_firing_interval / SV_Status.FiringSpeed();
            AK_Potential.UpdateShootingInterval(interval);
        }
    }
}
