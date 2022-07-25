using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DE_Availability : WeaponControllerComponent
    {
        static public readonly int maxAmmoInMag = 7;

        static public readonly float preparingTime = 0.83f;
        static public readonly float shootingInterval = 0.14f;

        static public bool Available { get; private set; }

        static float shootingIntervalRemain;
        static float preparingTimeRemain;

        static public int MaxAmmoInMag { get; } = maxAmmoInMag;
        static public int AmmoInMag { get; set; } = maxAmmoInMag;

        public override void Initialize()
        {
            SetEvent(1);
        }

        public override void Shutdown()
        {
            SetEvent(-1);
        }

        public override void Activate()
        {
            preparingTimeRemain = preparingTime / SV_Status.WeaponSpeed();
            shootingIntervalRemain = 0.0f;
        }

        public override void Inactivate()
        {

        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                WeaponController.Shot += BeginCooldown;
            }

            else
            {
                WeaponController.Shot -= BeginCooldown;
            }
        }

        static void BeginCooldown(object obj, Vector3 direction)
        {
            shootingIntervalRemain = shootingInterval;
        }

        public override void Update(float dt)
        {
            preparingTimeRemain -= dt;
            shootingIntervalRemain -= dt;

            if (preparingTimeRemain < 0.0f) { preparingTimeRemain = 0.0f; }
            if (shootingIntervalRemain < 0.0f) { shootingIntervalRemain = 0.0f; }

            if (AmmoInMag <= 0) { Available = false; return; }

            if (preparingTimeRemain > 0.0f || shootingIntervalRemain > 0.0f)
            {
                Available = false;
            }

            else
            {
                Available = true;
            }
        }
    }
}

