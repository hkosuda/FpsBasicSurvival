using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ArmorItem : FieldItem
    {
        private void Awake()
        {
            Init(Item.armor);
        }

        protected override bool ItemMethod()
        {
            if (SV_Status.CurrentArmor >= SV_Status.CurrentMaxArmor)
            {
                return false;
            }

            else
            {
                SV_Status.RepairArmor(SvParams.GetInt(SvParam.field_armor_amount));
                return true;
            }
        }
    }
}

