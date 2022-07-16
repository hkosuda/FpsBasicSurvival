using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem_ReplenishAmmo : ShopItems
{
    private void Awake()
    {
        AwakeMethod(ShopItem.replenish_ammo, 
            Ints.Item.shop_item_replenish_ammo_amount, 
            Ints.Item.shop_item_replenish_ammo_cost_default, 
            Ints.Item.shop_item_replenish_ammo_cost_increase);
    }

    private void Start()
    {
        var currentCost = CurrentCost();

        if (SV_StatusAdmin.StatusList[SV_Status.money] >= currentCost)
        {
            SV_ShopAdmin.AddToCart(ShopItem.replenish_ammo);
        }
    }

    protected override void UpdateContent(object obj, bool mute)
    {
        var currentCost = CurrentCost();
        var totalCost = TotalCost();

        if (Ints.Get(amount) < 0)
        {
            nameText.text = name;
            UpdateAddButton_Inf();
        }

        else
        {
            nameText.text = name + " (+" + Ints.Get(amount).ToString() + ")";
            UpdateAddButton_Fin();
        }

        numberText.text = "x " + Utility.GetDividedNumberText(SV_ShopAdmin.CartList[item]).ToString();
        currentCostText.text = Utility.GetDividedNumberText(currentCost).ToString();
        totalCostText.text = Utility.GetDividedNumberText(totalCost).ToString();

        UpdateSubButton();
    }

    void UpdateAddButton_Inf()
    {
        if (SV_ShopAdmin.MoneyRemain < CurrentCost() || SV_ShopAdmin.CartList[item] > 0)
        {
            addButton.interactable = false;
        }

        else
        {
            addButton.interactable = true;
        }
    }

    void UpdateAddButton_Fin()
    {
        if (SV_ShopAdmin.MoneyRemain < CurrentCost())
        {
            addButton.interactable = false;
            return;
        }

        var currentAmmo = AkController.AmmoInMag + AkController.AmmoInBag;
        var nextAmmo = currentAmmo + Ints.Get(amount) * SV_ShopAdmin.CartList[item];
        var nextMaxAmmo = SV_ShopAdmin.NextMaxAmmoInMag + SV_ShopAdmin.NextMaxAmmoInBag;

        if (nextAmmo >= nextMaxAmmo)
        {
            addButton.interactable = false;
        }

        else
        {
            addButton.interactable = true;
        }
    }

    protected override string GetCurrentValueString()
    {
        return "-";
    }

    protected override string GetNextValueString()
    {
        return "-";
    }

    protected override string GetDiscription()
    {
        if (Ints.Get(Ints.Item.shop_item_replenish_ammo_amount) < 0)
        {
            var discription = "　弾薬を補充します．補充されるのはAKMの弾薬で，Desert Eagleの弾薬は自動で補充されます．\n" +
                "　このアップグレートは，ひとつ購入するだけでAKMの弾薬がすべて補充されます．";

            return discription;
        }

        else
        {
            var discription = "　弾薬を補充します．補充されるのはAKMの弾薬だけです．Desert Eagleの弾薬は自動で補充されます．";

            return discription;
        }
    }
}
