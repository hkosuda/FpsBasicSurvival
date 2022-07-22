using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemTimeRemain : ShopItemButton
    {
        float _currentValue;
        float _nextValue;

        private void Awake()
        {
            Initialize(ShopItem.time_remain);
        }

        protected override string CalcCurrentValue()
        {
            _currentValue = SV_Time.TimeRemain + SvParams.Get(SvParam.additional_time_after_round);
            return TxtUtil.Time(_currentValue, true, ".");
        }

        protected override string CalcNextValue()
        {
            CalcCurrentValue();

            var nCart = SV_ShopItem.CartList[Item];
            _nextValue = _currentValue + nCart * increase;

            return TxtUtil.Time(_nextValue, true, ".");
        }

        protected override string Description()
        {
            return "écÇËéûä‘Çëùâ¡Ç≥ÇπÇ‹Ç∑ÅD";
        }

        protected override void Apply()
        {
            var nCart = SV_ShopItem.CartList[Item];
            var additional = nCart * increase;

            SV_Time.SetAdditionalTime(additional);
        }
    }
}
