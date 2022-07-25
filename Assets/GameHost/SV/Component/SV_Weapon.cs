using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_Weapon : HostComponent
    {
        public override void Initialize()
        {
            SetEvent(1);
        }

        public override void Shutdown()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                TimerSystem.Updated += Replenish;
            }

            else
            {
                TimerSystem.Updated += Replenish;
            }
        }

        static void Replenish(object obj, float dt)
        {
            if (SV_Round.RoundNumber == 0)
            {
                AK_Availability.MaxAmmoInMag = Const.ak_defaultMaxAmmoInMag;
                AK_Availability.MaxAmmoInBag = Const.ak_defaultMaxAmmoInBag;

                AK_Availability.AmmoInBag = AK_Availability.MaxAmmoInBag;
                DE_Availability.AmmoInMag = DE_Availability.MaxAmmoInMag;
            }
        }
    }
}

