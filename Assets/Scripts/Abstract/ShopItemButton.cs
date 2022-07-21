using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public abstract class ShopItemButton : MonoBehaviour
    {
        static readonly public Dictionary<ShopItem, string> itemNames = new Dictionary<ShopItem, string>()
        {
            { ShopItem.hp, "体力" },
            { ShopItem.armor, "アーマー" },
            { ShopItem.max_hp, "体力の最大値" },
            { ShopItem.max_armor, "アーマー最大値" },
            { ShopItem.damage_rate, "ダメージ率" },
            { ShopItem.money_rate, "獲得マネー倍率" },
            { ShopItem.ammo_in_mag, "装弾数" },
            { ShopItem.ammo_in_bag, "携帯弾数" },
            { ShopItem.time_remain, "残り時間" },
        };

        static readonly public Dictionary<ShopItem, int> increaseList = new Dictionary<ShopItem, int>()
        {
            { ShopItem.hp, 100 },
            { ShopItem.armor, 100 },
            { ShopItem.max_hp, 50 },
            { ShopItem.max_armor, 50 },
            { ShopItem.damage_rate, 15 },
            { ShopItem.money_rate, 15 },
            { ShopItem.ammo_in_mag, 3 },
            { ShopItem.ammo_in_bag, 15 },
            { ShopItem.time_remain, 15 },
        };

        static readonly public Dictionary<ShopItem, int> costDefaultList = new Dictionary<ShopItem, int>()
        {
            { ShopItem.hp, 100 },
            { ShopItem.armor, 100 },
            { ShopItem.max_hp, 1000 },
            { ShopItem.max_armor, 1500 },
            { ShopItem.damage_rate, 2000 },
            { ShopItem.money_rate, 2500 },
            { ShopItem.ammo_in_mag, 1000 },
            { ShopItem.ammo_in_bag, 1500 },
            { ShopItem.time_remain, 1000 },
        };

        static readonly public Dictionary<ShopItem, int> costIncreaseList = new Dictionary<ShopItem, int>()
        {
            { ShopItem.hp, 50 },
            { ShopItem.armor, 100 },
            { ShopItem.max_hp, 800 },
            { ShopItem.max_armor, 1000 },
            { ShopItem.damage_rate, 1200 },
            { ShopItem.money_rate, 1800 },
            { ShopItem.ammo_in_mag, 1000 },
            { ShopItem.ammo_in_bag, 1500 },
            { ShopItem.time_remain, 1000 },
        };

        // ui components
        protected TextMeshProUGUI nameText;
        protected TextMeshProUGUI currentValueText;
        protected TextMeshProUGUI nextValueText;
        protected TextMeshProUGUI addText;
        protected TextMeshProUGUI currentCostText;
        protected TextMeshProUGUI numberText;
        protected TextMeshProUGUI totalCostText;

        protected Button addButton;
        protected Button subButton;

        // status
        public ShopItem Item { get; private set; }
        protected int increase;
        protected int costDefault;
        protected int costIncrease;

        // readable
        protected int currentValue;
        protected int nextValue;

        protected void Initialize(ShopItem item)
        {
            Item = item;
            increase = increaseList[item];
            costDefault = costDefaultList[item];
            costIncrease = costIncreaseList[item];

            var mainButton = gameObject.GetComponent<Button>();
            mainButton.onClick.AddListener(UpdateDesctiption);

            nameText = GetText(0);
            currentValueText = GetText(1);
            nextValueText = GetText(2);
            addText = GetText(3);
            currentCostText = GetText(4);
            // spacer (5)
            addButton = GetButton(6);
            // spacer (7)
            subButton = GetButton(8);
            // spacer (9)
            numberText = GetText(10);
            totalCostText = GetText(11);

            addButton.onClick.AddListener(AddToCart);
            subButton.onClick.AddListener(SubFromCart);

            SV_ShopItem.AddMyself(this);

            // - inner function
            TextMeshProUGUI GetText(int n)
            {
                return gameObject.transform.GetChild(n).gameObject.GetComponent<TextMeshProUGUI>();
            }

            // - inner function
            Button GetButton(int n)
            {
                return gameObject.transform.GetChild(n).gameObject.GetComponent<Button>();
            }
        }

        public void OnDestroy()
        {
            Apply();
        }

        public int CurrentCost()
        {
            var level = SV_ShopItem.LevelList[Item];
            var nInCart = SV_ShopItem.CartList[Item];

            return costDefault + costIncrease * (level + nInCart);
        }

        public int TotalCost()
        {
            var level = SV_ShopItem.LevelList[Item];
            var nInCart = SV_ShopItem.CartList[Item];

            var total = 0;

            for (var n = 0; n < nInCart; n++)
            {
                total += costDefault + costIncrease * (level + n);
            }

            return total;
        }

        public virtual void UpdateContent()
        {
            var currentCost = CurrentCost();
            var totalCost = TotalCost();

            nameText.text = itemNames[Item];
            addText.text = "+" + increase.ToString("#,0");
            currentValueText.text = CalcCurrentValue();
            nextValueText.text = CalcNextValue();
            numberText.text = "x " + SV_ShopItem.CartList[Item].ToString("#,0");
            currentCostText.text = currentCost.ToString("#,0");
            totalCostText.text = totalCost.ToString("#,0");

            UpdateAddButton();
            UpdateSubButton();

            // - inner function
            void UpdateSubButton()
            {
                subButton.interactable = (SV_ShopItem.CartList[Item] > 0);
            }

            void UpdateAddButton()
            {
                addButton.interactable = CheckAddToCart();
            }
        }

        protected virtual bool CheckAddToCart()
        {
            if (SV_ShopItem.MoneyRemain < CurrentCost())
            {
                return false;
            }

            return true;
        }

        protected void AddToCart()
        {
            SV_ShopItem.AddToCart(Item);
        }

        protected void SubFromCart()
        {
            SV_ShopItem.SubFromCart(Item);
        }

        protected void UpdateDesctiption()
        {
            Shop_Description.ShowDescription(itemNames[Item], Description());
        }

        protected abstract string CalcCurrentValue();
        protected abstract string CalcNextValue();
        protected abstract string Description();
        protected abstract void Apply();
    }
}

