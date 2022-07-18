using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_Time : HostComponent
    {
        static public float ActiveTime { get; private set; }
        static public float TimeRemain { get; private set; }

        public override void Initialize()
        {
            ActiveTime = 0.0f;
            SetEvent(1);
        }

        public override void Begin()
        {

        }

        public override void Stop()
        {
            ActiveTime = 0.0f;
        }

        public override void Shutdown()
        {
            SetEvent(-1);
        }

        void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                TimerSystem.Updated += UpdateMethod;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
            }
        }

        void UpdateMethod(object obj, float dt)
        {
            ActiveTime += dt;
        }

        static public void SetTimeRemain(float timeRemain)
        {
            TimeRemain = timeRemain;
        }
    }
}

