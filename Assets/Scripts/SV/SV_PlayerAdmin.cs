using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_PlayerAdmin : HostComponent
    {
        static GameObject player;

        public SV_PlayerAdmin()
        {
            player = Player.Myself;
        }

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
            player.transform.position = ShareSystem.Point2Position(SV_GoalStartAdmin.StartPoint);
        }

        public override void Stop()
        {

        }

        
    }
}

