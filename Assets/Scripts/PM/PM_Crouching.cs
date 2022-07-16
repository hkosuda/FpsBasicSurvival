using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class PM_Crouching : Controller
    {
        static readonly float playerMinSize = 0.5f;

        static public bool IsCrouching { get; private set; }

        static public float PlayerSize { get; private set; } = 1.0f;

        public override void Update(float dt)
        {
            if (Keyconfig.CheckInput(Keyconfig.KeyAction.crouch, false))
            {
                IsCrouching = true;

                PlayerSize -= Params.pm_crouching_speed * dt;
                if (PlayerSize < playerMinSize) { PlayerSize = playerMinSize; }
            }

            else
            {
                IsCrouching = false;

                PlayerSize += Params.pm_crouching_speed * dt;
                if (PlayerSize > 1.0f) { PlayerSize = 1.0f; }
            }

            Player.Myself.transform.localScale = new Vector3(1.0f, PlayerSize, 1.0f);
        }
    }
}

