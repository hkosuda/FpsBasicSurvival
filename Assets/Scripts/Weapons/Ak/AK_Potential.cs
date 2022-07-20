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
                shootingInterval: Const.ak_firing_interval,
                resetTime: 0.2f,
                lifting: 1.2f,
                h_random: 0.35f,
                v_random: 0.2f,
                h_running: 2.0f,
                v_running: 1.8f,
                liftingExpo: 2.5f,
                randomExpo: 2.5f,
                runningExpo: 1.2f, 
                spreadPattern: new List<float>() 
                {
                    1, 1, // 1 - 2
                    -1, -1, -1, // 3 - 5
                    1, 1, 1, 1, // 6 - 9
                    -1, -1, -1, -1, -1, -1, // 10 - 14
                }
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

