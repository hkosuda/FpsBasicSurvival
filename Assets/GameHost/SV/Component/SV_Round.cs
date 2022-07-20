using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_Round : HostComponent
    {
        static public int RoundNumber { get; private set; }

        static public int MazeRow { get; private set; } = 13;
        static public int MazeCol { get; private set; } = 13;

        //static public int MazeRow { get; private set; } = 4;
        //static public int MazeCol { get; private set; } = 3;

        static public int NumberOfEnemies { get; private set; } = 20;
        static public int CurrentKey { get; set; }

        public override void Initialize()
        {
            SvParams.Initialize();
            RoundNumber = -1;
        }

        public override void Shutdown()
        {

        }

        public override void Begin()
        {
            RoundNumber++;
            CurrentKey = 0;
        }

        public override void Stop()
        {
            RoundNumber = -1;
        }
    }
}

