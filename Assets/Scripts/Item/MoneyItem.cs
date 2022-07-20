using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class MoneyItem : FieldItem
    {
        private void Awake()
        {
            Init(Item.money);
        }

        protected override bool ItemMethod()
        {
            SV_Status.AddMoney(SvParams.GetInt(SvParam.field_money_amount));
            return true;
        }
    }
}

