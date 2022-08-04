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
            Initialize(ShopItem.max_armor,
                SvParams.GetInt(SvParam.shop_max_armor_amount),
                SvParams.GetInt(SvParam.shop_max_armor_cost_default),
                SvParams.GetInt(SvParam.shop_max_armor_cost_increase));
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
            return "�A�[�}�[�̍ő�l�𑝉������܂��D\n" +
                "�A�[�}�[���Ȃ���Ԃł́C�G�̍U�����󂯂����ɔ��ɑ傫�ȃ_���[�W���󂯂Ă��܂��܂��D" +
                "�ϋɓI�ɃA�b�v�O���[�h���s���܂��傤�D";
        }

        protected override void Apply()
        {
            SV_Status.SetMaxArmor(nextValue);
        }
    }
}
