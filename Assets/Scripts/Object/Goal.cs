using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [RequireComponent(typeof(BoxCollider))]
    public class Goal : MonoBehaviour
    {
        static public EventHandler<bool> GameClear { get; set; }

        static public EventHandler<bool> InsufficientKeys { get; set; }

        private void Start()
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != Const.playerLayer) { return; }

            if (SV_Round.RoundNumber == 0)
            {
                TimerSystem.Pause();
                SV_ShopItem.BeginShopping();
            }

            else
            {
                if (SV_Round.CurrentKey < SvParams.GetInt(SvParam.require_keys))
                {
                    SVUI_Message.ShowAlert("キーの数が不十分です");
                }

                else
                {
                    if (SV_Round.RoundNumber == SvParams.GetInt(SvParam.clear_round))
                    {
                        GameClear?.Invoke(null, false);

                        TimerSystem.Pause();
                        SV_History.ShowHistoryOnClear();
                    }

                    else
                    {
                        TimerSystem.Pause();
                        SV_ShopItem.BeginShopping();
                    }
                }
            }
        }
    }
}

