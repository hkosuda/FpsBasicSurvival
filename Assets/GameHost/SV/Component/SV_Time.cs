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

        public override void Initialize()
        {
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
                TimeRemain += SvParams.Get(SvParam.additional_time_after_round);
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

                if (TimeRemain > 60.0f)
                {
                    var text = TxtUtil.C(TxtUtil.Time(TimeRemain, false), Clr.lime);
                    SVUI_Time.UpdateText(text);
                }

                else
                {
                    if (TimeRemain < 30.0f)
                    {
                        var text = TxtUtil.C(TxtUtil.SecMSec(TimeRemain), Clr.red);
                        SVUI_Time.UpdateText(text);
                    }

                    else
                    {
                        var text = TxtUtil.C(TxtUtil.SecMSec(TimeRemain), Clr.orange);
                        SVUI_Time.UpdateText(text);
                    }
                }

                if (TimeRemain <= 0.0f)
                {
                    var text = TxtUtil.C(TxtUtil.SecMSec(0.0f), Clr.red);
                    SVUI_Time.UpdateText(text);

                    TimeOut?.Invoke(null, false);
                }
            }
        }

        static public void AdditionalTime(float additionalTime)
        {
            TimeRemain += additionalTime;
        }
    }
}

