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
            if (!Keyconfig.CheckInput(KeyAction.shot, true)) { return; }
            if (!DE_Availability.Available) { return; }

            DeShot();
        }

        static void DeShot()
        {
            var ray = GetRay();

            WeaponController.Shot?.Invoke(null, ray.direction);

            if (Physics.Raycast(ray, hitInfo: out RaycastHit hit))
            {
                WeaponController.ShootingHit?.Invoke(null, hit);
            }

            // - inner function
            static Ray GetRay()
            {
                Debug.Log(DE_Potential.SpreadParam.potential);

                return new Ray()
                {
                    origin = Player.Camera.transform.position,
                    direction = SpreadSolver.CalcSpread(DE_Potential.SpreadParam),
                };
            }
        }
    }
}


