using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class PM_Camera : Controller
    {
        static Transform tr;

        static public float addRotX;
        static public float addRotY;

        static float degRotX;
        static float degRotY;

        public override void Initialize()
        {
            tr = Player.Camera.transform;
            tr.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }

        public override void Update(float dt)
        {
            var dx = Input.GetAxis("Mouse Y") * Params.mouse_sens;
            var dy = Input.GetAxis("Mouse X") * Params.mouse_sens;

            degRotX -= dx;
            degRotY += dy;

            var rotX = degRotX + addRotX;

            if (rotX > 90.0f) { degRotX = 90.0f - addRotX; rotX = 90.0f; }
            if (rotX < -90.0f) { degRotX = -90.0f - addRotX; rotX = -90.0f; }

            tr.eulerAngles = new Vector3(rotX, degRotY, 0.0f);
        }

        static public Vector3 EulerAngle()
        {
            return tr.eulerAngles;
        }

        static public void SetEulerAngles(Vector3 euler)
        {
            tr.eulerAngles = euler;

            degRotX = euler.x;
            degRotY = euler.y;
        }
    }
}
