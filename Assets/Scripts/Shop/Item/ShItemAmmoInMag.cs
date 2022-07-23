using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemAmmoInMag : ShopItemButton
    {
        private void Awake()
        {
            Initialize(ShopItem.ammo_in_mag);
        }

        protected override string CalcCurrentValue()
        {
            currentValue = AK_Availability.MaxAmmoInMag;
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
            return "ëïíeêîÇëùâ¡Ç≥ÇπÇ‹Ç∑ÅD";
        }

        protected override void Apply()
        {
            CalcNextValue();

            AK_Availability.MaxAmmoInMag = nextValue;
            AK_Availability.AmmoInMag = AK_Availability.MaxAmmoInMag;
            DE_Availability.AmmoInMag = DE_Availability.MaxAmmoInMag;
        }
    }
}

