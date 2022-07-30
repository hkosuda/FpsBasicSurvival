using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [RequireComponent(typeof(EnemyMain))]
    public class EnemyChat : MonoBehaviour
    {
        static EventHandler<EnemyChat> MineSendYes { get; set; }

        EnemyMain main;

        private void Awake()
        {
            main = gameObject.GetComponent<EnemyMain>();
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                GameHost.HostBegan += SendGLHF;

                Goal.GameClear += SendGG;
                EnemyMain.EnemyDestroyed += SendF;

                MineMain.MineExplosion += SendYes;
                MineSendYes += ReplyYes;

                SV_Time.TimeUp += SendNoobOnTimeUp;
                SV_Status.PlayerDead += SendNoobOnDead;
            }

            else
            {
                GameHost.HostBegan -= SendGLHF;

                Goal.GameClear -= SendGG;
                EnemyMain.EnemyDestroyed -= SendF;

                MineMain.MineExplosion -= SendYes;
                MineSendYes -= ReplyYes;

                SV_Time.TimeUp -= SendNoobOnTimeUp;
                SV_Status.PlayerDead -= SendNoobOnDead;
            }
        }

        void SendGLHF(object obj, bool mute)
        {
            if (SV_Round.RoundNumber != 1) { return; }

            var probability = 0.2f;
            var min = 0.5f;
            var max = 4.5f;

            if (Random(probability))
            {
                var message = "glhf";

                if (Random(0.3f))
                {
                    message = message.ToUpper();
                }

                DelayedChatSystem.AddMessage(Header(main) + message, Delay(min, max));
            }
        }

        void SendGG(object obj, bool mute)
        {
            var probability = 0.5f;
            var min = 1.5f;
            var max = 8.0f;

            if (Random(probability))
            {
                var message = Header(main) + "gg";
                var delay = UnityEngine.Random.Range(min, max);

                DelayedChatSystem.AddMessage(message, delay);
            }
        }

        void SendF(object obj, EnemyMain _main)
        {
            var probability = 0.02f;
            var min = 1.0f;
            var max = 2.0f;

            if (_main.gameObject == gameObject)
            {
                if (Random(probability))
                {
                    var message = Header(main) + "****";
                    DelayedChatSystem.AddMessage(message, Delay(min, max));
                    return;
                }

                if (Random(probability))
                {
                    var message = Header(main) + "guys, he is cheating";
                    DelayedChatSystem.AddMessage(message, Delay(min, max));
                    return;
                }

                if (Random(probability))
                {
                    var message = Header(main) + "ns";
                    DelayedChatSystem.AddMessage(message, Delay(min, max));
                    return;
                }
            }
        }

        void SendYes(object obj, MineMain _main)
        {
            var probability = 0.1f;
            var min = 0.5f;
            var max = 1.0f;

            if (_main.gameObject == gameObject)
            {
                if (Random(probability))
                {
                    var message = Header(main) + LongYes();
                    DelayedChatSystem.AddMessage(message, Delay(min, max));
                    MineSendYes?.Invoke(null, this);
                }
            }

            // - inner function
            static string LongYes()
            {
                var num = UnityEngine.Random.Range(3, 12);
                var yes = "Y";

                for (var n = 0; n < num; n++)
                {
                    yes += "E";
                }

                return yes + "S";
            }
        }

        void ReplyYes(object obj, EnemyChat chat)
        {
            var probability = 0.1f;
            var min = 1.2f; // must be later than "Send Yes"
            var max = 1.8f;

            if (chat.gameObject != gameObject)
            {
                if (Random(probability))
                {
                    var message = Header(main) + "nc";
                    DelayedChatSystem.AddMessage(message, Delay(min, max));
                    return;
                }

                if (Random(probability))
                {
                    var message = Header(main) + "gj";
                    DelayedChatSystem.AddMessage(message, Delay(min, max));
                    return;
                }
            }
        }

        void SendNoobOnTimeUp(object obj, bool mute)
        {
            var probability = 0.3f;
            var min = 0.5f;
            var max = 2.0f;

            if (Random(probability))
            {
                var message = Header(main) + "noob";
                DelayedChatSystem.AddMessage(message, Delay(min, max));
                return;
            }

            if (Random(probability))
            {
                var message = Header(main) + "ez";
                DelayedChatSystem.AddMessage(message, Delay(min, max));
                return;
            }

            if (Random(probability))
            {
                var message = Header(main) + "???";
                DelayedChatSystem.AddMessage(message, Delay(min, max));
                return;
            }

            if (Random(probability))
            {
                var message = Header(main) + "...";
                DelayedChatSystem.AddMessage(message, Delay(min, max));
                return;
            }

            if (Random(probability))
            {
                var message = Header(main) + "wtf";
                DelayedChatSystem.AddMessage(message, Delay(min, max));
                return;
            }
        }

        void SendNoobOnDead(object obj, bool mute)
        {
            var probability = 0.3f;
            var min = 0.5f;
            var max = 2.0f;

            if (Random(probability))
            {
                var message = Header(main) + "noob";
                DelayedChatSystem.AddMessage(message, Delay(min, max));
                return;
            }

            if (Random(probability))
            {
                var message = Header(main) + "ez";
                DelayedChatSystem.AddMessage(message, Delay(min, max));
                return;
            }

            if (Random(probability))
            {
                var message = Header(main) + "gg";
                DelayedChatSystem.AddMessage(message, Delay(min, max));
                return;
            }

            if (Random(probability))
            {
                var message = Header(main) + "lol";
                DelayedChatSystem.AddMessage(message, Delay(min, max));
                return;
            }
        }

        //
        // utility functions

        static string Header(EnemyMain main)
        {
            return TxtUtil.C(SVUI_KillLogManager.DeadName(main) + " : ", Clr.cyan);
        }

        static bool Random(float probability)
        {
            var rnd = UnityEngine.Random.Range(0.0f, 1.0f);

            if (rnd > 1.0f - probability)
            {
                return true;
            }

            return false;
        }

        static float Delay(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}

