using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SVUI_KillLogManager : MonoBehaviour
    {
        static public readonly int maxLogs = 10;
        static public readonly float logExistTime = 4.0f;

        static List<GameObject> logList;
        static GameObject myself;

        private void Awake()
        {
            myself = gameObject;
            logList = new List<GameObject>();
            SVUI_KillLog.Initialize();
        }

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
            if (indicator > 0)
            {
                EnemyMain.EnemyDestroyed += AddKillLog;
            }

            else
            {
                EnemyMain.EnemyDestroyed -= AddKillLog;
            }
        }

        static void AddKillLog(object obj, EnemyMain enemyMain)
        {
            var log = GetLog(enemyMain);
            log.transform.SetParent(myself.transform);
            log.transform.SetSiblingIndex(0);

            logList.Add(log);

            if (logList.Count > maxLogs)
            {
                RemoveKillLog();
            }

            // - inner function 
            static GameObject GetLog(EnemyMain enemyMain)
            {
                string killer;
                string dead = enemyMain.EnemyType.ToString() + "_" + Mathf.Abs(enemyMain.GetInstanceID()).ToString();

                if (enemyMain.KilledBy == Killer.akm)
                {
                    killer = "AKM";
                }

                else if (enemyMain.KilledBy == Killer.deagle)
                {
                    killer = "Deagle";
                }

                else if (enemyMain.KilledBy == Killer.myself)
                {
                    killer = dead;
                }

                else
                {
                    killer = "Player";
                }

                return SVUI_KillLog.InstantiateKillLog(killer, dead);
            }
        }

        static public void RemoveKillLog()
        {
            if (logList.Count == 0) { return; }

            if (logList[0] == null)
            {
                logList.RemoveAt(0);
                return;
            }

            GameObject.Destroy(logList[0]);
            logList.RemoveAt(0);
        }
    }
}

