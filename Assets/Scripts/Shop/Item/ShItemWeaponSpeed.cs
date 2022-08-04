using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemWeaponSpeed : ShopItemButton
    {
        static readonly string extension = " [%]";

        private void Awake()
        {
            Initialize(ShopItem.weapon_speed,
                SvParams.GetInt(SvParam.shop_weapon_speed_amount),
                SvParams.GetInt(SvParam.shop_weapon_speed_cost_default),
                SvParams.GetInt(SvParam.shop_weapon_speed_cost_increase));
        }

        protected override string CalcCurrentValue()
        {
            currentValue = SV_Status.WeaponSpeedRate;
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
            return "武器の持ち替えや，リロードのスピードを上昇させます．\n" +
                "武器の持ち替え時やリロード時は非常に危険です．そうした危険な時間を短縮できるため，積極的に検討しましょう．";
        }

        protected override void Apply()
        {
            SV_Status.SetWeaponSpeedRate(nextValue);
        }
    }
}
