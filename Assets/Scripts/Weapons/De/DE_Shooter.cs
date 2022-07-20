using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DE_Shooter : WeaponControllerComponent
    {

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        public override void Update(float dt)
        {
            if (Keyconfig.CheckInput(KeyAction.shot, true))
            {
                if (DE_Availability.AmmoInMag == 0)
                {
                    WeaponController.Empty?.Invoke(null, false);
                }

                else
                {
                    if (DE_Availability.Available)
                    {
                        DeShot();
                    }
                }
            }
        }

        static void DeShot()
        {
            var ray = GetRay();

            WeaponController.Shot?.Invoke(null, ray.direction);
            DE_Availability.AmmoInMag -= 1;

            if (Physics.Raycast(ray, hitInfo: out RaycastHit hit))
            {
                WeaponController.ShootingHit?.Invoke(null, hit);
            }

            // - inner function
            static Ray GetRay()
            {
                return new Ray()
                {
                    origin = Player.Camera.transform.position,
                    direction = SpreadSolver.CalcSpread(DE_Potential.SpreadParam),
                };
            }
        }
    }
}


