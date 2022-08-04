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
            return "�c�莞�Ԃ𑝉������܂��D\n" +
                "�c�莞�Ԃ𑝉������邱�Ƃ́C�^�C���A�b�v�̊댯�����炷�����łȂ����E���h���Ƀ}�l�[�𓾂���@��𑝂₷���Ƃɂ��Ȃ���܂��D";
        }

        protected override void Apply()
        {
            SV_Time.AdditionalTime(additional);
        }
    }
}
