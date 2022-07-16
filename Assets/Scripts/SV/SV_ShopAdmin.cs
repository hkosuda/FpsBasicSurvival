using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum ShopItem
    {
        hp_healing,
        armor_repairing,
        hp_upgrade,
        armor_upgrade,
        damage_rate_booster,
        money_rate_booster,
        mag_extension,
        bag_extension,
        replenish_ammo,
    }

    public class SV_ShopAdmin : HostComponent
    {
        static public EventHandler<bool> CalcTotalCostBegin { get; set; }
        static public EventHandler<bool> CartUpdated { get; set; }

        static public Dictionary<ShopItem, int> CartList;
        static public Dictionary<ShopItem, int> LevelList;

        // status
        static public int CurrentMoney { get; private set; }
        static public int MoneyRemain { get; private set; }

        // next values
        static public int NextHP { get; private set; }
        static public int NextArmor { get; private set; }
        static public int NextDamageRate { get; private set; }
        static public int NextMoneyRate { get; private set; }
        static public int NextAmmoInMag { get; private set; }
        static public int NextAmmoInBag { get; private set; }

        // next max values
        static public int NextMaxHP { get; private set; }
        static public int NextMaxArmor { get; private set; }
        static public int NextMaxAmmoInMag { get; private set; }
        static public int NextMaxAmmoInBag { get; private set; }

        // utility
        static public int TotalCost { get; private set; }

        public SV_ShopAdmin()
        {
            LevelList = new Dictionary<ShopItem, int>();
            CartList = new Dictionary<ShopItem, int>();

            foreach (ShopItem item in Enum.GetValues(typeof(ShopItem)))
            {
                LevelList.Add(item, 0);
                CartList.Add(item, 0);
            }
        }

        public override void Begin()
        {
            var _shopPanel = Resources.Load<GameObject>("UI/SV_Shop");
            GameObject.Instantiate(_shopPanel);
        }

        public override void Stop()
        {

        }

        public override void Shutdown()
        {

        }

        static public void SubMoneyRemain(int totalCost)
        {
            MoneyRemain -= totalCost;
            TotalCost += totalCost;
        }

        static public void SubFromCart(ShopItem shopItem)
        {
            CartList[shopItem]--;
            AfterUpdateProcessings();
        }

        static public void AddToCart(ShopItem shopItem)
        {
            CartList[shopItem]++;
            AfterUpdateProcessings();
        }

        static public void Initialize()
        {
            foreach (ShopItem item in Enum.GetValues(typeof(ShopItem)))
            {
                CartList[item] = 0;
            }

            CurrentMoney = SV_StatusAdmin.StatusList[SV_Status.money];
            MoneyRemain = SV_StatusAdmin.StatusList[SV_Status.money];

            AfterUpdateProcessings();
        }

        static void AfterUpdateProcessings()
        {
            UpdateNextMaxValues();
            CorrectCart();

            CalcTotalCost();
            CalcNextValues();

            CartUpdated?.Invoke(null, false);
        }

        static void UpdateNextMaxValues()
        {
            NextMaxHP = GetNextValue(ShopItem.hp_upgrade, SV_StatusAdmin.CurrentMaxHP, Params.shop_hp_upgrade_amount);
            NextMaxArmor = GetNextValue(ShopItem.armor_upgrade, SV_StatusAdmin.CurrentMaxArmor, Params.shop_armor_upgrade_amount);
            //NextMaxAmmoInMag = GetNextValue(ShopItem.mag_extension, AkController.MaxAmmoInMag, Params.shop_mag_extension_amount);
            //NextMaxAmmoInBag = GetNextValue(ShopItem.bag_extension, AkController.MaxAmmoInBag, Params.shop_bag_extension_amount);
        }

        static void CalcNextValues()
        {
            NextHP = GetNextValue_Clip(ShopItem.hp_healing, SV_StatusAdmin.StatusList[SV_Status.hp], Params.shop_hp_healing_amount, NextMaxHP);
            NextArmor = GetNextValue_Clip(ShopItem.armor_repairing, SV_StatusAdmin.StatusList[SV_Status.armor], Params.shop_armor_repairing_amount, NextMaxArmor);
            NextDamageRate = GetNextValue(ShopItem.damage_rate_booster, SV_StatusAdmin.CurrentDamageRate, Params.shop_damage_rate_booster_amount);
            NextMoneyRate = GetNextValue(ShopItem.money_rate_booster, SV_StatusAdmin.CurrentMoneyRate, Params.shop_money_rate_booster_amount);
        }

        static int GetNextValue(ShopItem item, int currentValue, int amount)
        {
            var n_inCart = CartList[item];

            return currentValue + amount * n_inCart;
        }

        static int GetNextValue_Clip(ShopItem item, int currentValue, int amount, int maxValue)
        {
            var n_inCart = CartList[item];

            var nextValue = currentValue + amount * n_inCart;

            return (int)Calcf.Clip(0, maxValue, nextValue);
        }

        static void CorrectCart()
        {
            SubFromCart(ShopItem.hp_healing, NextMaxHP, SV_StatusAdmin.StatusList[SV_Status.hp], Params.shop_hp_healing_amount);
            SubFromCart(ShopItem.armor_repairing, NextMaxArmor, SV_StatusAdmin.StatusList[SV_Status.armor], Params.shop_armor_repairing_amount);

            // - inner function
            static void SubFromCart(ShopItem item, int maxValue, int currentValue, int amount)
            {
                var level = LevelList[item];

                while (true)
                {
                    var nextValue = currentValue + amount * (level + CartList[item] - 1);

                    if (nextValue >= maxValue)
                    {
                        CartList[item]--;
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        static void CalcTotalCost()
        {
            MoneyRemain = SV_StatusAdmin.StatusList[SV_Status.money];
            TotalCost = 0;

            CalcTotalCostBegin?.Invoke(null, false);
        }

        static public void ReflectUpgrades()
        {
            SV_StatusAdmin.StatusList[SV_Status.hp] = NextHP;
            SV_StatusAdmin.StatusList[SV_Status.armor] = NextArmor;
            SV_StatusAdmin.StatusList[SV_Status.money] = Mathf.RoundToInt(MoneyRemain * Params.money_increase_after_round);

            SV_StatusAdmin.CurrentMaxHP = NextMaxHP;
            SV_StatusAdmin.CurrentMaxArmor = NextMaxArmor;

            SV_StatusAdmin.CurrentDamageRate = NextDamageRate;
            SV_StatusAdmin.CurrentMoneyRate = NextMoneyRate;

            //AkController.AmmoInMag = NextAmmoInMag;
            //AkController.AmmoInBag = NextAmmoInBag;
        }
    }
}

