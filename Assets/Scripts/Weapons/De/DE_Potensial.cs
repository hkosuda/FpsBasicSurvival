using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DE_Potensial : Controller
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
                lifting: 0.8f,
                h_random: 1.8f,
                v_random: 1.0f,
                h_jumping: 1.0f,
                v_jumping: 1.6f,
                liftingExpo: 1.5f,
                randomExpo: 1.5f,
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
                DE_Shooter.Shot += IncreasePotential;
            }

            else
            {
                DE_Shooter.Shot -= IncreasePotential;
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

