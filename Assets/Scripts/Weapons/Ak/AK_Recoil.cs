using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class AK_Recoil : WeaponControllerComponent
    {
        public override void Initialize()
        {
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
                WeaponController.Shot += StartRecoilling;
            }

            else
            {
                WeaponController.Shot -= StartRecoilling;
            }
        }

        static void StartRecoilling(object obj, Vector3 direction)
        {
            LinerDampingSolver.Initialize();
        }

        public override void Update(float dt)
        {
            if (!LinerDampingSolver.Active) { return; }

            LinerDampingSolver.UpdateTime(dt);

            var addRot = LinerDampingSolver.GetPosition();

            PM_Camera.addRotX = addRot[0];
            PM_Camera.addRotY = addRot[1];
        }
    }
}

