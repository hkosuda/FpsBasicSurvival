using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_Round : HostComponent
    {
        static public int RoundNumber { get; private set; }
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

        }
    }
}

