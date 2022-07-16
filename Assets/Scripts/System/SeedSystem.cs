using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SeedSystem : MonoBehaviour
    {
        static public int Seed { get; private set; }

        private void Awake()
        {
            Seed = -1;
        }

        static public void SetSeed(int seed = 0)
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

