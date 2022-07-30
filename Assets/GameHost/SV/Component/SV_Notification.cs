using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_Notification : HostComponent
    {
        static int destroyCounter = 0;
        static float damageCounter = 0.0f;

        public override void Initialize()
        {
            destroyCounter = 0;
            damageCounter = 0.0f;

            SetEvent(1);
        }

        public override void Shutdown()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                EnemyMain.EnemyDestroyed += ShowDamageHistory;
                SV_Status.PlayerDamageTaken += ShowPlayerDamage;

                EnemyMain.EnemyDestroyed += IncrementDestroyCounter;
                Goal.GameClear += SendEZ;
            }

            else
            {
                EnemyMain.EnemyDestroyed -= ShowDamageHistory;
                SV_Status.PlayerDamageTaken -= ShowPlayerDamage;

                EnemyMain.EnemyDestroyed -= IncrementDestroyCounter;
                Goal.GameClear -= SendEZ;
            }
        }

        static void ShowDamageHistory(object obj, EnemyMain main)
        {
            if (SV_Round.RoundNumber == 0) { return; }

            if (main.DamageHistory != null)
            {
                var last = "shots";
                if (main.DamageHistory.Count == 1) { last = "shot"; }

                var message = Header() + "Player1 destroyed " + SVUI_KillLogManager.DeadName(main) + " in " + main.DamageHistory.Count.ToString() + " " + last + " : ";

                for(var n = 0; n < main.DamageHistory.Count; n++)
                {
                    var damage = main.DamageHistory[n];

                    if (n == main.DamageHistory.Count - 1)
                    {
                        message += damage.ToString("F1");
                    }

                    else
                    {
                        message += damage.ToString("F1") + " / ";
                    }
                }

                ChatMessageManager.SendChatMessage(message);
            }

            // - inner function
            static string Header()
            {
                return TxtUtil.C("[Report] ", Clr.lime);
            }
        }

        static void ShowPlayerDamage(object obj, int[] hpArmor)
        {
            if (SV_Round.RoundNumber == 0) { return; }

            var message = Header() + "Player1 received damage ( HP : -" + hpArmor[0].ToString() + ", Armor : -" + hpArmor[1].ToString() + " )";
            ChatMessageManager.SendChatMessage(message);

            // - inner function
            static string Header()
            {
                return TxtUtil.C("[Report] ", Clr.red);
            }
        }

        static void IncrementDestroyCounter(object obj, EnemyMain main)
        {
            if (SV_Round.RoundNumber == 0) { return; }
            destroyCounter++;

            if (destroyCounter % 50 == 0)
            {
                var message = Header() + "Player1 destroyed " + destroyCounter.ToString() + " enemies.";
                ChatMessageManager.SendChatMessage(message);
            }

            // - inner function
            static string Header()
            {
                return TxtUtil.C("[Information] ", Clr.lime);
            }
        }

        static void SendEZ(object obj, bool mute)
        {
            ChatMessageManager.SendChatMessage(TxtUtil.C("Player1 : ", Clr.orange) + "gg");
        }
    }
}

