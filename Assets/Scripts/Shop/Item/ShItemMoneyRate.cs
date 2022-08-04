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
            return "獲得するマネーの量を増加させます．\n" +
                "アップグレードの価格は上昇しつづけます．そのため，マネー獲得率を上昇させてアップグレードを継続的に購入できるようにしましょう．";
        }

        protected override void Apply()
        {
            SV_Status.SetMoneyRate(nextValue);
        }
    }
}
