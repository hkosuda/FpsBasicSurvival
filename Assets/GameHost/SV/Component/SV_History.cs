using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_History : HostComponent
    {
        public enum Condition
        {
            timeout, dead, clear
        }

        public enum HistoryValue
        {
            movingDistance, takenDamage, shotAmmo,
        }
        
        static public Condition CurrentCondition { get; private set; }

        static public History CurrentHistory { get; private set; }
        static public List<History> HistoryList { get; private set; }

        static GameObject _historyView;

        public override void Initialize()
        {
            _historyView = Resources.Load<GameObject>("UI/SvHistory");

            HistoryList = new List<History>();
            CurrentHistory = new History();

            SetEvent(1);
        }

        public override void Shutdown()
        {
            HistoryList = null;
            CurrentHistory = null;

            SetEvent(-1);
        }

        public override void Begin()
        {
            CurrentHistory = new History();
            HistoryList.Add(CurrentHistory);
        }

        public override void Stop()
        {
            CurrentHistory = null;
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                Player.Moved += UpdateMovingDistance;
                SV_Status.PlayerDamageTaken += UpdateDamage;

                WeaponController.Shot += UpdateShotAmmo;

                SV_Status.PlayerDead += ShowHistroyOnDead;
                SV_Time.TimeOut += ShowHistoryOnTimeout;
            }

            else
            {
                Player.Moved -= UpdateMovingDistance;
                SV_Status.PlayerDamageTaken -= UpdateDamage;

                WeaponController.Shot -= UpdateShotAmmo;

                SV_Status.PlayerDead -= ShowHistroyOnDead;
                SV_Time.TimeOut -= ShowHistoryOnTimeout;
            }
        }

        //
        // update info

        static void UpdateMovingDistance(object obj, float delta)
        {
            CurrentHistory.valueList[HistoryValue.movingDistance] += delta;
        }

        static void UpdateDamage(object obj, int damage)
        {
            CurrentHistory.valueList[HistoryValue.takenDamage] += damage;
        }

        static void UpdateShotAmmo(object obj, Vector3 direction)
        {
            CurrentHistory.valueList[HistoryValue.shotAmmo]++;
        }

        //
        // show history

        static void ShowHistroyOnDead(object obj, bool mute)
        {
            CurrentCondition = Condition.dead;
            ShowHistory();
        }

        static void ShowHistoryOnTimeout(object obj, bool mute)
        {
            CurrentCondition = Condition.timeout;
            ShowHistory();
        }

        static void ShowHistory()
        {
            TimerSystem.Pause();
            GameHost.Instantiate(_historyView);
        }

        public class History
        {
            public Dictionary<HistoryValue, float> valueList;
            public Dictionary<ShopItem, int> buyList;

            public History()
            {
                valueList = new Dictionary<HistoryValue, float>();

                foreach (HistoryValue hv in Enum.GetValues(typeof(HistoryValue)))
                {
                    valueList.Add(hv, 0);
                }

                buyList = new Dictionary<ShopItem, int>();

                foreach(ShopItem shopItem in Enum.GetValues(typeof(ShopItem)))
                {
                    buyList.Add(shopItem, 0);
                }
            }
        }
    }
}

