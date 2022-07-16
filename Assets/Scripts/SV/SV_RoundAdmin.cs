using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_RoundAdmin : HostComponent
    {
        static public int RoundNumber { get; private set; }

        static public int MazeRow { get; private set; } = 13;
        static public int MazeCol { get; private set; } = 13;

        static public int NumberOfEnemies { get; private set; } = 40;

        public SV_RoundAdmin()
        {
            RoundNumber = -1;
        }

        public override void Initialize()
        {
            RoundNumber = -1;
        }

        public override void Shutdown()
        {

        }

        public override void Begin()
        {
            RoundNumber++;
        }

        public override void Stop()
        {
            RoundNumber = -1;
        }
    }
}

