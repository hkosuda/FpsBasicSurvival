using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemAmmoInMag : ShopItemButton
    {
        private void Awake()
        {
            Initialize(ShopItem.ammo_in_mag,
                SvParams.GetInt(SvParam.shop_mag_extension_amount),
                SvParams.GetInt(SvParam.shop_mag_extension_cost_default),
                SvParams.GetInt(SvParam.shop_mag_extension_cost_increase));
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
            return "" +
                "���e���𑝉������܂��D\n" +
                "���e���𑝉������邱�Ƃ́C�����[�h�̉񐔂����炷���Ƃɂ��Ȃ���܂��D" +
                "�����[�h���͖��h���ƂȂ邽�߁C���̉񐔂����炷���Ƃ͊댯�����炷���Ƃɂ��Ȃ���ł��傤�D";
        }

        protected override void Apply()
        {
            AK_Availability.MaxAmmoInMag = nextValue;
            AK_Availability.AmmoInMag = AK_Availability.MaxAmmoInMag;
            DE_Availability.AmmoInMag = DE_Availability.MaxAmmoInMag;
        }
    }
}

