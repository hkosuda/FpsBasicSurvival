using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class AK_Availability : WeaponControllerComponent
    {
        static public readonly float preparingTime = 1.0f;

        static public bool Available { get; private set; }

        static public float ShootingIntervalRemain { get; private set; }
        static public float PreparingTimeRemain { get; private set; }

        static public int AmmoInMag { get; set; } = 30;
        static public int AmmoInBag { get; set; } = 150;

        static public int MaxAmmoInMag { get; set; } = 30;
        static public int MaxAmmoInBag { get; set; } = 150;

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
            PreparingTimeRemain = preparingTime / SV_Status.WeaponSpeed();
            ShootingIntervalRemain = 0.0f;
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
            ShootingIntervalRemain = Const.ak_firing_interval / SV_Status.FiringSpeed();
        }

        public override void Update(float dt)
        {
            PreparingTimeRemain -= dt;
            ShootingIntervalRemain -= dt;

            if (PreparingTimeRemain < 0.0f) { PreparingTimeRemain = 0.0f; }
            if (ShootingIntervalRemain < 0.0f) { ShootingIntervalRemain = 0.0f; }

            if (AmmoInMag <= 0) { Available = false; return; }

            if (PreparingTimeRemain > 0.0f || ShootingIntervalRemain > 0.0f)
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

