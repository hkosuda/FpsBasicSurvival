using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class PM_Observer : Controller
    {
        static readonly float observerSpeed = 150.0f;
        static readonly float observerAccel = 400.0f;

        static float currentSpeed;

        public override void Update(float dt)
        {
            if (!ObserverCommand.Active) { currentSpeed = 0.0f; return; }

            PM_Main.Interrupt = true;
            Player.Rb.velocity = Vector3.zero;

            var pos = Player.Myself.transform.position;
            var vec = GetVector();

            if (vec.magnitude > 0.0f)
            {
                currentSpeed += observerAccel * dt;
            }

            else
            {
                currentSpeed = 0.0f;
            }

            if (currentSpeed > observerSpeed) { currentSpeed = observerSpeed; }

            Player.Myself.transform.position = pos + GetVector() * currentSpeed * dt;
        }

        static Vector3 GetVector()
        {
            var vm = PM_InputVector.ML_InputVector.x;
            var vl = PM_InputVector.ML_InputVector.y;

            var rotX = -PM_Camera.EulerAngle().x * Mathf.Deg2Rad;
            var rotY = PM_Camera.EulerAngle().y * Mathf.Deg2Rad;

            var vz = vm * Mathf.Cos(rotX) * Mathf.Cos(rotY) - vl * Mathf.Sin(rotY);
            var vx = vm * Mathf.Cos(rotX) * Mathf.Sin(rotY) + vl * Mathf.Cos(rotY);
            var vy = vm * Mathf.Sin(rotX);

            return new Vector3(vx, vy, vz).normalized;
        }
    }
}

