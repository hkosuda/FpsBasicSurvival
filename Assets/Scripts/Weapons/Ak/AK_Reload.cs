using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class AK_Reload : WeaponControllerComponent
    {
        static readonly float reloadingTime = 3.083f;
        static readonly float replenishTime = 1.833f;

        static public EventHandler<bool> ReloadingBegin { get; set; }
        static public bool IsReloading { get; private set; }

        static float pastTime;
        static bool replenishEnd;

        public override void Update(float dt)
        {
            if (IsReloading)
            {
                pastTime += dt;

                if (pastTime > replenishTime && !replenishEnd)
                {
                    replenishEnd = true;
                    AK_Availability.AmmoInMag = AK_Availability.MaxAmmoInMag;
                }

                if (pastTime > reloadingTime)
                {
                    IsReloading = false;
                }
            }

            else
            {
                if (AK_Availability.PreparingTimeRemain <= 0.0f && AK_Availability.AmmoInMag < AK_Availability.MaxAmmoInMag)
                {
                    if (Keyconfig.CheckInput(KeyAction.reload, true))
                    {
                        ReloadingBegin?.Invoke(null, false);
                        IsReloading = true;

                        pastTime = 0.0f;
                        replenishEnd = false;
                    }
                }
            }
        }
    }
}

