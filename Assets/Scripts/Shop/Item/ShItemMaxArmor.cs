using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemMaxArmor : ShopItemButton
    {
        static public int NextMaxArmor { get; private set; }

        private void Awake()
        {
            Initialize(ShopItem.max_armor);
        }

        protected override string CalcCurrentValue()
        {
            currentValue = SV_Status.CurrentMaxArmor;
            return currentValue.ToString();
        }

        protected override string CalcNextValue()
        {
            CalcCurrentValue();

            var nCart = SV_ShopItem.CartList[Item];
            nextValue = currentValue + nCart * increase;

            NextMaxArmor = nextValue;
            return nextValue.ToString();
        }

        protected override string Description()
        {
            return "ëÃóÕÇÃç≈ëÂílÇëùâ¡Ç≥ÇπÇ‹Ç∑ÅD";
        }

        protected override void Apply()
        {
            CalcNextValue();
            SV_Status.CurrentMaxArmor = nextValue;
        }
    }
}
