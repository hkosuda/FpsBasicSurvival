using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemAmmoInBag : ShopItemButton
    {
        private void Awake()
        {
            Initialize(ShopItem.ammo_in_bag);
        }

        protected override string CalcCurrentValue()
        {
            currentValue = AK_Availability.MaxAmmoInBag;
            return currentValue.ToString();
        }

        protected override string CalcNextValue()
        {
            CalcCurrentValue();

            var nCart = SV_ShopItem.CartList[Item];
            nextValue = currentValue + nCart * increase;

            return nextValue.ToString();
        }

        protected override string Description()
        {
            return "�g�ђe���𑝉������܂��D";
        }

        protected override void Apply()
        {
            CalcNextValue();
            AK_Availability.MaxAmmoInBag = nextValue;
        }
    }
}