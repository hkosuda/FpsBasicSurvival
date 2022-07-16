using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public abstract class ShopItems : MonoBehaviour
    {
        static public Dictionary<ShopItem, string> ItemNames = new Dictionary<ShopItem, string>()
        {
            { ShopItem.hp_healing, "ëÃóÕâÒïú" },
            { ShopItem.armor_repairing, "ÉAÅ[É}Å[èCëU" },
            { ShopItem.hp_upgrade, "ëÃóÕã≠âª" },
            { ShopItem.armor_upgrade, "ÉAÅ[É}Å[ã≠âª" },
            { ShopItem.damage_rate_booster, "É_ÉÅÅ[ÉWó¶ëùâ¡" },
            { ShopItem.money_rate_booster, "älìæÉ}ÉlÅ[î{ó¶ëùâ¡" },
            { ShopItem.mag_extension, "íeëqägí£" },
            { ShopItem.bag_extension, "ågë—íeñÚëùâ¡" },
            { ShopItem.replenish_ammo, "íeñÚï‚è[" },
        };

        // ui components
        protected TextMeshProUGUI nameText;
        protected TextMeshProUGUI addText;
        protected TextMeshProUGUI currentValueText;
        protected TextMeshProUGUI nextValueText;
        protected TextMeshProUGUI numberText;
        protected TextMeshProUGUI currentCostText;
        protected TextMeshProUGUI totalCostText;

        protected Button addButton;
        protected Button subButton;

        // status
        protected string name;
        protected ShopItem item;
        protected int amount;
        protected int cost_default;
        protected int cost_increase;

        protected void AwakeMethod(ShopItem item, int amount, int cost_default, int cost_increase)
        {
            name = ItemNames[item];
            this.item = item;
            this.amount = amount;
            this.cost_default = cost_default;
            this.cost_increase = cost_increase;

            var mainButton = gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
            mainButton.onClick.AddListener(UpdateDesctiption);

            nameText = GetText(0);
            addText = GetText(1);
            currentValueText = GetText(2);
            nextValueText = GetText(3);
            addButton = GetButton(4);
            subButton = GetButton(5);
            numberText = GetText(6);
            currentCostText = GetText(7);
            totalCostText = GetText(8);

            addButton.onClick.AddListener(AddToCart);
            subButton.onClick.AddListener(SubFromCart);

            SetEvent(1);

            // - inner function
            TextMeshProUGUI GetText(int n)
            {
                return gameObject.transform.GetChild(0).GetChild(n).gameObject.GetComponent<TextMeshProUGUI>();
            }

            // - inner function
            Button GetButton(int n)
            {
                return gameObject.transform.GetChild(0).GetChild(n).gameObject.GetComponent<Button>();
            }
        }

        protected void OnDestroy()
        {
            SetEvent(-1);
        }

        protected void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                SV_ShopAdmin.CalcTotalCostBegin += CalcTotalCost;
                SV_ShopAdmin.CartUpdated += UpdateContent;
            }

            else
            {
                SV_ShopAdmin.CalcTotalCostBegin -= CalcTotalCost;
                SV_ShopAdmin.CartUpdated -= UpdateContent;
            }
        }

        protected virtual int CurrentCost()
        {
            var level = SV_ShopAdmin.LevelList[item];
            var n_inCart = SV_ShopAdmin.CartList[item];

            var _cost_default = cost_default;
            var _cost_increase = cost_increase;

            return _cost_default + _cost_increase * (level + n_inCart);
        }

        protected virtual int TotalCost()
        {
            var level = SV_ShopAdmin.LevelList[item];
            var n_cart = SV_ShopAdmin.CartList[item];

            var _cost_default = cost_default;
            var _cost_increase = cost_increase;

            var total = 0;

            for (var n = 1; n <= n_cart; n++)
            {
                total += _cost_default + _cost_increase * (level + n - 1);
            }

            return total;
        }

        protected virtual void CalcTotalCost(object obj, bool mute)
        {
            SV_ShopAdmin.SubMoneyRemain(TotalCost());
        }

        protected virtual void UpdateContent(object obj, bool mute)
        {
            var currentCost = CurrentCost();
            var totalCost = TotalCost();

            nameText.text = name;
            addText.text = "+" + amount.ToString("#,0");
            currentValueText.text = GetCurrentValueString();
            nextValueText.text = GetNextValueString();
            numberText.text = "x " + SV_ShopAdmin.CartList[item].ToString("#,0");
            currentCostText.text = currentCost.ToString("#,0");
            totalCostText.text = totalCost.ToString("#,0");

            UpdateAddButton();
            UpdateSubButton();
        }

        protected void UpdateSubButton()
        {
            if (SV_ShopAdmin.CartList[item] == 0)
            {
                subButton.interactable = false;
            }

            else
            {
                subButton.interactable = true;
            }
        }

        protected void UpdateAddButton()
        {
            if (CheckAddToCart())
            {
                addButton.interactable = true;
            }

            else
            {
                addButton.interactable = false;
            }
        }

        protected virtual bool CheckAddToCart()
        {
            if (SV_ShopAdmin.MoneyRemain < CurrentCost())
            {
                return false;
            }

            return true;
        }

        protected virtual void AddToCart()
        {
            SV_ShopAdmin.AddToCart(item);
        }

        protected virtual void SubFromCart()
        {
            SV_ShopAdmin.SubFromCart(item);
        }

        protected void UpdateDesctiption()
        {
            ShopPanel_Message.UpdateDiscription(item, GetDescription());
        }

        protected abstract string GetCurrentValueString();
        protected abstract string GetNextValueString();
        protected abstract string GetDescription();
    }
}

