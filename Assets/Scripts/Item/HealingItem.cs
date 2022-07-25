using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class HealingItem : FieldItem
    {
        private void Awake()
        {
            Init(Item.healing);
        }

        protected override bool ItemMethod()
        {
            if (SV_Status.CurrentHP >= SV_Status.CurrentMaxHP)
            {
                SVUI_Message.ShowAlert("体力が最大のため，取得できません");
                return false;
            }

            else
            {
                SV_Status.Heal(SvParams.GetInt(SvParam.field_hp_amount));
                return true;
            }
        }
    }
}

