using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DE_Potential : WeaponControllerComponent
    {
        static public readonly float maxPotential = 0.8f;
        static public readonly float potentialIncrease = 0.3f;

        static public SpreadParam SpreadParam { get; private set; }

        public override void Initialize()
        {
            SpreadParam = new SpreadParam(
                maxPotential: 0.8f,
                potentialIncrease: 0.3f,
                shootingInterval: 0.1f,
                resetTime: 0.3f,
                lifting: 0.6f,
                h_random: 0.9f,
                v_random: 1.2f,
                h_running: 3.0f,
                v_running: 2.2f,
                liftingExpo: 1.5f,
                randomExpo: 1.5f,
                runningExpo: 0.7f
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

