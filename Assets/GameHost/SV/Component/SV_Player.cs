using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_Player : HostComponent
    {
        static GameObject player;

        public override void Initialize()
        {
            player = Player.Myself;
        }

        public override void Shutdown()
        {
            player = null;
        }

        public override void Begin()
        {
            if (SV_Round.RoundNumber == 0)
            {
                Player.SetPosition(Vector3.zero, Vector3.zero);
            }

            else
            {
                Player.SetPosition(ShareSystem.Point2Position(SV_GoalStart.StartPoint), Vector3.zero);
            }
        }

        public override void Stop()
        {

        }
    }
}

