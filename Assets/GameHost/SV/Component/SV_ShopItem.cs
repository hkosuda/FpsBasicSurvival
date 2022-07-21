using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum ShopItem
    {
        hp, armor, 
        max_hp, max_armor,
        damage_rate, money_rate,
        ammo_in_mag, ammo_in_bag,
        time_remain
    }

    public class SV_ShopItem : HostComponent
    {
        static public EventHandler<bool> TotalCostCalcEnd { get; set; }

        static public Dictionary<ShopItem, int> CartList;
        static public Dictionary<ShopItem, int> LevelList;

        static public int MoneyRemain { get; private set; }

        static Dictionary<ShopItem, ShopItemButton> itemList;

        // utility
        static GameObject _shop;
        static GameObject shop;

        public override void Initialize()
        {
            _shop = Resources.Load<GameObject>("UI/SV_Shop");
            InitializeLevelList();

            // - inner function
            static void InitializeLevelList()
            {
                LevelList = new Dictionary<ShopItem, int>();

                foreach(ShopItem shopItem in Enum.GetValues(typeof(ShopItem)))
                {
                    LevelList.Add(shopItem, 0);
                }
            }
        }

        public override void Shutdown()
        {
            _shop = null;
        }

        public override void Begin()
        {
            LevelList[ShopItem.hp] = SV_Round.RoundNumber;
            LevelList[ShopItem.armor] = SV_Round.RoundNumber;

            CartList = new Dictionary<ShopItem, int>();

            foreach (ShopItem item in Enum.GetValues(typeof(ShopItem)))
            {
                CartList.Add(item, 0);
            }

            itemList = new Dictionary<ShopItem, ShopItemButton>();

            shop = GameHost.Instantiate(_shop);
            shop.SetActive(false);
        }

        public override void Stop()
        {
            foreach (var item in CartList)
            {
                if (item.Key == ShopItem.hp || item.Key == ShopItem.armor) { continue; }

                LevelList[item.Key] += item.Value;
            }

            SV_Status.SetMoney(MoneyRemain);
        }

        static public void BeginShopping()
        {
            shop.SetActive(true);
            UpdateCart();
        }

        static public void AddMyself(ShopItemButton shopItems)
        {
            if (itemList == null) { itemList = new Dictionary<ShopItem, ShopItemButton>(); }

            itemList.Add(shopItems.Item, shopItems);
        }

        static public void AddToCart(ShopItem shopItem)
        {
            CartList[shopItem]++;
            UpdateCart();
        }

        static public void SubFromCart(ShopItem shopItem)
        {
            if (CartList[shopItem] > 0)
            {
                CartList[shopItem]--;
            }

            UpdateCart();
        }

        static void UpdateCart()
        {
            MoneyRemain = SV_Status.CurrentMoney;

            foreach(var item in itemList)
            {
                MoneyRemain -= item.Value.TotalCost();
            }

            UpdateContent();
            TotalCostCalcEnd?.Invoke(null, false);

            // - inner function
            static void UpdateContent()
            {
                // must be run in order (do not use 'foreach'. dictionary's order is unkown)
                itemList[ShopItem.max_hp].UpdateContent();
                itemList[ShopItem.max_armor].UpdateContent();
                itemList[ShopItem.hp].UpdateContent();
                itemList[ShopItem.armor].UpdateContent();
                itemList[ShopItem.damage_rate].UpdateContent();
                itemList[ShopItem.money_rate].UpdateContent();
                itemList[ShopItem.ammo_in_mag].UpdateContent();
                itemList[ShopItem.ammo_in_bag].UpdateContent();
                itemList[ShopItem.time_remain].UpdateContent();
            }
        }

        static public void ClearCart()
        {
            foreach(ShopItem shopItem in Enum.GetValues(typeof(ShopItem)))
            {
                CartList[shopItem] = 0;
            }

            UpdateCart();
        }
    }
}

