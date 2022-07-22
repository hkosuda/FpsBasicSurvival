using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_Time : HostComponent
    {
        static public EventHandler<bool> TimeOut { get; set; }

        static public float TimeRemain { get; private set; }
        static public float AdditionalTime { get; private set; }

        public override void Initialize()
        {
            AdditionalTime = 0.0f;
            SetEvent(1);
        }

        public override void Begin()
        {
            if (SV_Round.RoundNumber == 0)
            {
                TimeRemain = 0.0f;
            }

            else
            {
                TimeRemain += SvParams.Get(SvParam.additional_time_after_round) + AdditionalTime; Debug.Log(TimeRemain);
                AdditionalTime = 0.0f;
            }
        }

        public override void Stop()
        {

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
            if (SV_Round.RoundNumber > 0)
            {
                TimeRemain -= dt;

                if (TimeRemain <= 0.0f)
                {
                    Debug.Log(TimeRemain);
                    TimeOut?.Invoke(null, false);
                }
            }
        }

        static public void SetAdditionalTime(float additionalTime)
        {
            AdditionalTime = additionalTime;
        }
    }
}

