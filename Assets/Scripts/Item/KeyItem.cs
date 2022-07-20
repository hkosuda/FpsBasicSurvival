using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class KeyItem : FieldItem
    {
        private void Awake()
        {
            Init(Item.key);
        }

        protected override bool ItemMethod()
        {
            SV_Round.CurrentKey++;
            return true;
        }
    }
}

