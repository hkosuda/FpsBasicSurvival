using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class AkController : WeaponController
    {
        public AkController(Weapon weapon) : base(weapon)
        {
            controllerList = new List<WeaponControllerComponent>()
            {
                new AK_Availability(),
                new AK_Shooter(),
                new AK_Potential(),
                new AK_Recoil(),
            };
        }

    }
}

