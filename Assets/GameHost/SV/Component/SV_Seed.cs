using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_Seed : HostComponent
    {
        static public int BaseSeed { get; private set; } = 3000;
        static public int Seed { get; private set; }

        public override void Initialize()
        {
            Seed = BaseSeed;
        }

        public override void Shutdown()
        {

        }

        public override void Begin()
        {
            Seed = BaseSeed + SV_Round.RoundNumber;
        }

        static public void Init(int seed = 0)
        {
            if (Seed > 0)
            {
                UnityEngine.Random.InitState(Seed + seed);
            }

            else
            {
                var now = DateTime.Now.Millisecond;

                if (seed > 0)
                {
                    UnityEngine.Random.InitState(now + seed);
                }

                else
                {
                    UnityEngine.Random.InitState(now);
                }
            }
        }
    }
}

