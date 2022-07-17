using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DeController : WeaponController
    {
        public DeController(Weapon weapon) : base(weapon)
        {
            controllerList = new List<WeaponControllerComponent>()
            {
                new DE_Availability(),
                new DE_Shooter(),
                new DE_Potential(),
                new DE_Recoil(),
            };
        }
    }
}

