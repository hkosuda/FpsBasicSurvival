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
            player.transform.position = ShareSystem.Point2Position(SV_GoalStart.StartPoint);
        }

        public override void Stop()
        {

        }
    }
}

