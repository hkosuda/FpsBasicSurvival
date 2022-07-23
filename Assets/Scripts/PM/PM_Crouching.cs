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
            if (Keyconfig.CheckInput(KeyAction.crouch, false))
            {
                PlayerSize -= Params.pm_crouching_speed * dt;
                if (PlayerSize < playerMinSize) { PlayerSize = playerMinSize; }

                if (PM_Landing.LandingIndicator >= 0)
                {
                    IsCrouching = true;

                    var v = Player.Rb.velocity;
                    Player.Rb.velocity = new Vector3(v.x, -10.0f, v.z);
                }
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

