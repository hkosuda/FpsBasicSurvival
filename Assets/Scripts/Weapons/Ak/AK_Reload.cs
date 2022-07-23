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

                if (pastTime > replenishTime / SV_Status.WeaponSpeed() && !replenishEnd)
                {
                    replenishEnd = true;
                    Reload();
                }

                if (pastTime > reloadingTime / SV_Status.WeaponSpeed())
                {
                    IsReloading = false;
                }
            }

            else
            {
                if (AK_Availability.PreparingTimeRemain > 0.0f) { return; }

                if (AK_Availability.AmmoInBag > 0 && AK_Availability.AmmoInMag < AK_Availability.MaxAmmoInMag)
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

        public override void Inactivate()
        {
            IsReloading = false;
        }

        static void Reload()
        {
            var inBag = AK_Availability.AmmoInBag;
            var inMag = AK_Availability.AmmoInMag;

            var require = AK_Availability.MaxAmmoInMag - inMag;

            if (require > inBag)
            {
                AK_Availability.AmmoInMag = inBag;
                AK_Availability.AmmoInBag = 0;
            }

            else
            {
                AK_Availability.AmmoInMag += require;
                AK_Availability.AmmoInBag -= require;
            }
        }
    }
}

