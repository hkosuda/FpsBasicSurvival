using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class AK_Potential : WeaponControllerComponent
    {
        static public readonly float maxPotential = 0.8f;
        static public readonly float potentialIncrease = 0.3f;

        static public SpreadParam SpreadParam { get; private set; }

        public override void Initialize()
        {
            SpreadParam = new SpreadParam(
                maxPotential: 0.6f,
                potentialIncrease: 0.06f,
                shootingInterval: 0.095f,
                lifting: 0.8f,
                h_random: 0.3f,
                v_random: 0.1f,
                h_running: 1.5f,
                v_running: 1.2f,
                liftingExpo: 2.5f,
                randomExpo: 2.5f,
                runningExpo: 0.5f
                );

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
                WeaponController.Shot += IncreasePotential;
            }

            else
            {
                WeaponController.Shot -= IncreasePotential;
            }
        }

        static void IncreasePotential(object obj, Vector3 direction)
        {
            SpreadParam.IncreasePotential();
        }

        public override void Update(float dt)
        {
            SpreadParam.DecreasePotential(dt);
        }
    }
}

