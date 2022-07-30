using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class SV_RoundText : HostComponent
    {
        static readonly float stableTime = 1.5f;
        static readonly float reductionSpeed = 0.2f;

        static readonly float initialAlpha = 0.5f;

        static GameObject _ui;
        static GameObject ui;

        static TextMeshProUGUI text;
        static float pastTime;

        public override void Initialize()
        {
            _ui = Resources.Load<GameObject>("UI/RoundText");
            SetEvent(1);
        }

        public override void Shutdown()
        {
            _ui = null;
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
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

        static void UpdateMethod(object obj, float dt)
        {
            if (ui == null || text == null) { return; }

            pastTime += dt;

            if (pastTime > stableTime)
            {
                var delta = pastTime - stableTime;
                var alpha = initialAlpha - reductionSpeed * delta;

                if (alpha < 0.0f)
                {
                    GameObject.Destroy(ui);
                }

                else
                {
                    text.color = new Color(1.0f, 1.0f, 1.0f, alpha);
                }
            }

            else
            {
                text.color = new Color(1.0f, 1.0f, 1.0f, initialAlpha);
            }
        }

        public override void Begin()
        {
            if (SV_Round.RoundNumber == 0) { return; }

            pastTime = 0.0f;

            ui = GameHost.Instantiate(_ui);
            text = ui.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            if (SV_Round.RoundNumber == SvParams.GetInt(SvParam.clear_round))
            {
                text.text = "Last Round";
            }

            else
            {
                text.text = "Round " + SV_Round.RoundNumber.ToString() + " / " + SvParams.GetInt(SvParam.clear_round).ToString();
            }
        }
    }
}

