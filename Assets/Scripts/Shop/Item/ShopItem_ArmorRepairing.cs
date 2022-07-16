using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShopItem_ArmorRepairing : ShopItems
    {
        private void Awake()
        {
            AwakeMethod(ShopItem.armor_repairing,
                Params.shop_armor_repairing_amount,
                Params.shop_armor_repairing_cost_default,
                Params.shop_armor_repairing_cost_increase);
        }

        protected override bool CheckAddToCart()
        {
            if (SV_ShopAdmin.MoneyRemain < CurrentCost())
            {
                return false;
            }

            var currentArmor = SV_StatusAdmin.StatusList[SV_Status.armor];

            var nextArmor = currentArmor + amount * SV_ShopAdmin.CartList[item];
            var nextMaxArmor = SV_ShopAdmin.NextMaxArmor;

            if (nextArmor >= nextMaxArmor)
            {
                return false;
            }

            return true;
        }

        protected override string GetCurrentValueString()
        {
            return SV_StatusAdmin.StatusList[SV_Status.armor].ToString();
        }

        protected override string GetNextValueString()
        {
            return SV_ShopAdmin.NextArmor.ToString();
        }

        protected override string GetDescription()
        {
            var constant = Params.damage_reduction_const;

            var discription = "　アーマーの耐久値を回復します．\n" +
                "　アーマーの耐久値が大きいほど受けるダメージは小さくなるため，耐久値が低い場合は回復しておくことをお勧めします．\n" +
                "（プレイヤーが受けるダメージは，与えられたダメージを '" + constant.ToString() + " / （アーマー耐久値 + " + constant.ToString() + "'倍" +
                "した値となります）";

            return discription;
        }
    }
}

