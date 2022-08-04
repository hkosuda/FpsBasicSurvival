using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShItemTimeRemain : ShopItemButton
    {
        float _currentValue;
        float _nextValue;

        float additional;

        private void Awake()
        {
            Initialize(ShopItem.time_remain,
                SvParams.GetInt(SvParam.shop_time_remain_amount),
                SvParams.GetInt(SvParam.shop_time_remain_cost_default),
                SvParams.GetInt(SvParam.shop_time_remain_cost_increase));
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
            additional = nCart * increase;

            _nextValue = _currentValue + additional;
            return TxtUtil.Time(_nextValue, true, ".");
        }

        protected override string Description()
        {
            return "残り時間を増加させます．\n" +
                "残り時間を増加させることは，タイムアップの危険を減らすだけでなくラウンド中にマネーを得られる機会を増やすことにもつながります．";
        }

        protected override void Apply()
        {
            SV_Time.AdditionalTime(additional);
        }
    }
}
