using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DebugChatSystem : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Start()
        {
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            //if (indicator > 0)
            //{
            //    SV_Status.PlayerDamageTaken += ShowPlayerDamage;
            //    EnemyMain.EnemyDamageTaken += ShowDamageInfo;
            //}

            //else
            //{
            //    SV_Status.PlayerDamageTaken -= ShowPlayerDamage;
            //    EnemyMain.EnemyDamageTaken -= ShowDamageInfo;
            //}
        }

        static void ShowPlayerDamage(object obj, int[] damage)
        {
            var message = "Player got damage : " + damage;
            ChatMessageManager.SendChatMessage(message);
        }

        static void ShowDamageInfo(object obj, float damage)
        {
            var message = "Enemy got damage : " + damage.ToString("F2");
            ChatMessageManager.SendChatMessage(message);
        }
#endif
    }
}

