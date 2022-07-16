using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DE_Shooter : Controller
    {
        static public EventHandler<Vector3> Shot { get; set; }
        static public EventHandler<RaycastHit> ShootingHit { get; set; }

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
            if (!Keyconfig.CheckInput(Keyconfig.KeyAction.shot, true)) { return; }
            if (!DE_Availability.Availability) { return; }

            DeShot();
        }

        static void DeShot()
        {
            var ray = GetRay();

            Shot?.Invoke(null, ray.direction);

            if (Physics.Raycast(ray, hitInfo: out RaycastHit hit))
            {
                ShootingHit?.Invoke(null, hit);
            }

            // - inner function
            static Ray GetRay()
            {
                return new Ray()
                {
                    origin = Player.Camera.transform.position,
                    direction = SpreadSolver.CalcSpread(DE_Potensial.SpreadParam),
                };
            }
        }
    }
}


