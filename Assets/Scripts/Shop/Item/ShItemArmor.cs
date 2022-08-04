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
            return "アーマーの耐久値を回復します．\n" +
                "プレイヤーが受けるダメージは，アーマーの耐久値が高いほど小さくなります．具体的に，被ダメージに対するHP減少量は以下の式で計算されます．\n" +
                "(HP減少量) = (被ダメージ) x 1000 / {1000 + (アーマー耐久値)}\n" +
                "なお，被ダメージに対するアーマー耐久値の減少量は常に被ダメージの50%です．";
        }

        protected override void Apply()
        {
            SV_Status.SetArmor(nextValue);
        }
    }
}

