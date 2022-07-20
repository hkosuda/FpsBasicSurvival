using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class AK_Shooter : WeaponControllerComponent
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
            if (AK_Reload.IsReloading) { return; }

            if (Keyconfig.CheckInput(KeyAction.shot, false))
            {
                if (AK_Availability.AmmoInMag == 0 && Keyconfig.CheckInput(KeyAction.shot, true))
                {
                    WeaponController.Empty?.Invoke(null, false);
                }

                else
                {
                    if (AK_Availability.Available)
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
            AK_Availability.AmmoInMag -= 1;

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
                    direction = SpreadSolver.CalcSpread(AK_Potential.SpreadParam),
                };
            }
        }
    }
}

