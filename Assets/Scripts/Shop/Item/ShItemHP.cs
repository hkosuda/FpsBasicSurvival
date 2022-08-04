using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemHP : ShopItemButton
    {
        private void Awake()
        {
            Initialize(ShopItem.hp,
                SvParams.GetInt(SvParam.shop_hp_amount),
                SvParams.GetInt(SvParam.shop_hp_cost_default),
                SvParams.GetInt(SvParam.shop_hp_cost_increase));
        }

        protected override string CalcCurrentValue()
        {
            currentValue = SV_Status.CurrentHP;
            return currentValue.ToString("#,0");
        }

        protected override string CalcNextValue()
        {
            currentValue = SV_Status.CurrentHP;

            var nCart = SV_ShopItem.CartList[Item];
            nextValue = currentValue + nCart * increase;

            var max = ShItemMaxHP.NextMaxHP;

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

            if (nextValue >= ShItemMaxHP.NextMaxHP)
            {
                return false;
            }

            return true;
        }

        protected override string Description()
        {
            return "体力を回復します．\n" +
                "体力がなくなればゲームオーバーです．減っているときは，必ず回復を行いましょう．";
        }

        protected override void Apply()
        {
            SV_Status.SetHP(nextValue);
        }
    }
}

