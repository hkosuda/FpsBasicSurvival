using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShopItemManager : MonoBehaviour
    {
        static GameObject myself;
        static GameObject _shopItem;

        private void Awake()
        {
            myself = gameObject;

            InstantiateShopItem<ShopItem_HpHealing>();
            InstantiateShopItem<ShopItem_ArmorRepairing>();
            InstantiateShopItem<ShopItem_HpUpgrade>();
            InstantiateShopItem<ShopItem_ArmorUpgrade>();
            InstantiateShopItem<ShopItem_DamageRateBooster>();
            InstantiateShopItem<ShopItem_MoneyRateBooster>();
            InstantiateShopItem<ShopItem_MagExtension>();
            InstantiateShopItem<ShopItem_BagExtension>();

            // - inner function 
            static void InstantiateShopItem<T>() where T : Component
            {
                if (_shopItem == null) { _shopItem = Resources.Load<GameObject>("UiComponent/ShopItem"); }

                var shopItem = Instantiate(_shopItem);
                shopItem.transform.SetParent(myself.transform);
                shopItem.AddComponent<T>();
            }
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }
}

