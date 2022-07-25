using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_Seed : HostComponent
    {
        static public int FixedSeedValue { get; private set; } = 3000;

        static public bool FixedSeed { get; private set; }
        static public int Seed { get; private set; }

        public override void Initialize()
        {
            if (FixedSeed)
            {
                Seed = FixedSeedValue;
            }

            else
            {
                Seed = DateTime.Now.Millisecond;
            }
        }

        public override void Shutdown()
        {

        }

        public override void Begin()
        {
            if (FixedSeed)
            {
                Seed = FixedSeedValue + SV_Round.RoundNumber;
            }

            else
            {
                Seed = DateTime.Now.Millisecond + SV_Round.RoundNumber;
            }
        }

        static public void SwitchSeedMode(bool status)
        {
            FixedSeed = status;
        }

        static public void SetFixedSeedValue(int seed)
        {
            FixedSeedValue = seed;
        }

        static public void Init(int add = 0)
        {
            UnityEngine.Random.InitState(Seed + add);
        }
    }
}

