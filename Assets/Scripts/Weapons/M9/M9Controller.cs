using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class M9Controller : WeaponController
    {
        public M9Controller(Weapon weapon) : base(weapon)
        {
            controllerList = new List<WeaponControllerComponent>()
            {
                new M9_Availability(),
            };
        }
    }
}

