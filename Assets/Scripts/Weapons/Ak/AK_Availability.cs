using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class AK_Availability : WeaponControllerComponent
    {
        static public readonly float preparingTime = 0.67f;
        static public readonly float shootingInterval = 0.095f;

        static public bool Available { get; private set; }

        static float shootingIntervalRemain;
        static float preparingTimeRemain;

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
            preparingTimeRemain = preparingTime;
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

