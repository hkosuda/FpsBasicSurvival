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
            _currentValue = SV_Time.TimeRemain;
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
            return "�c�莞�Ԃ𑝉������܂��D";
        }

        protected override void Apply()
        {
            CalcNextValue();
            SV_Time.SetTimeRemain(nextValue);
        }
    }
}