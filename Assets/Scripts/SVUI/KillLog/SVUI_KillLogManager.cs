using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SVUI_KillLogManager : MonoBehaviour
    {
        static public EventHandler<bool> LogUpdated { get; set; }

        static public readonly int maxLogs = 10;
        static public readonly float logExistTime = 4.0f;

        static GameObject myself;

        static List<GameObject> logList;
        static List<float> timeList;

        private void Awake()
        {
            myself = gameObject;

            logList = new List<GameObject>();
            timeList = new List<float>();

            KillLog.Initialize();
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
                EnemyMain.EnemyDestroyed += AddKillingLog;
                MineMain.MineExplosion += AddExplosionLog;

                TimerSystem.Updated += UpdateMethod;
            }

            else
            {
                EnemyMain.EnemyDestroyed -= AddKillingLog;
                MineMain.MineExplosion -= AddExplosionLog;
            }
        }

        static void UpdateMethod(object obj, float dt)
        {
            for(var n = logList.Count - 1; n > -1; n--)
            {
                timeList[n] += dt;

                if (timeList[n] > logExistTime)
                {
                    Destroy(logList[n]);

                    logList.RemoveAt(n);
                    timeList.RemoveAt(n);

                    LogUpdated?.Invoke(null, false);
                }
            }
        }

        static void AddKillingLog(object obj, EnemyMain enemyMain)
        {
            var log = KillLog.InstantiateLog("Player1", DeadName(enemyMain), GetHow());
            AddLog(log);

            // - inner function
            static KillLog.HowKilled GetHow()
            {
                var weapon = WeaponSystem.CurrentWeapon.Weapon;

                if (weapon == Weapon.ak) { return KillLog.HowKilled.ak; }
                return KillLog.HowKilled.de;
            }
        }

        static void AddExplosionLog(object obj, MineMain mine)
        {
            var log = KillLog.InstantiateLog(DeadName(mine), DeadName(mine), KillLog.HowKilled.explosion);
            AddLog(log);
        }

        static void AddLog(GameObject log)
        {
            log.transform.SetParent(myself.transform);
            log.transform.SetSiblingIndex(0);

            logList.Add(log);
            timeList.Add(0.0f);

            RemoveLogs();
            LogUpdated?.Invoke(null, false);

            // - inner function
            static void RemoveLogs()
            {
                while (true)
                {
                    if (logList.Count <= maxLogs) { return; }
                    Destroy(logList[0]);

                    logList.RemoveAt(0);
                    timeList.RemoveAt(0);
                }
            }
        }

        static string DeadName(EnemyMain enemyMain)
        {
            return enemyMain.EnemyType.ToString() + "_" + TxtUtil.PaddingZero3(enemyMain.brain.ID);
        }
    }
}

