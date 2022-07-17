using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_Seed : HostComponent
    {
        static public int Seed { get; private set; }

        public override void Initialize()
        {
            Seed = -1;
        }

        public override void Shutdown()
        {

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

