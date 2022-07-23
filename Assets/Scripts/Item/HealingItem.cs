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

