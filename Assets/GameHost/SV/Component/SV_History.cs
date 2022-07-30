using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_History : HostComponent
    {
        static public EventHandler<bool> HistoryUpdated { get; set; }

        public enum Condition
        {
            timeup, dead, clear
        }

        public enum HistoryValue
        {
            movingDistance, takenDamage, givenDamage, destroyed, shotAmmo, gotMoney
        }
        
        static public Condition CurrentCondition { get; private set; }
        static public List<History> HistoryList { get; private set; }

        static GameObject _historyView;

        public override void Initialize()
        {
            _historyView = Resources.Load<GameObject>("UI/SvHistory");
            HistoryList = new List<History>();

            SetEvent(1);
        }

        public override void Shutdown()
        {
            HistoryList = new List<History>();
            SetEvent(-1);
        }

        public override void Begin()
        {
            HistoryList.Add(new History());
        }

        public override void Stop()
        {
            
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                Player.Moved += UpdateMovingDistance;
                SV_Status.PlayerDamageTaken += UpdateTakeDamage;
                SV_Status.PlayerGotMoney += UpdateGotMoney;

                WeaponController.Shot += UpdateShotAmmo;

                SV_Status.PlayerDead += ShowHistroyOnDead;
                SV_Time.TimeUp += ShowHistoryOnTimeup;

                EnemyMain.EnemyDestroyed += UpdateDestroyed;
                EnemyMain.EnemyDamageTaken += UpdateGivenDamage;
            }

            else
            {
                Player.Moved -= UpdateMovingDistance;
                SV_Status.PlayerDamageTaken -= UpdateTakeDamage;
                SV_Status.PlayerGotMoney += UpdateGotMoney;

                WeaponController.Shot -= UpdateShotAmmo;

                SV_Status.PlayerDead -= ShowHistroyOnDead;
                SV_Time.TimeUp -= ShowHistoryOnTimeup;

                EnemyMain.EnemyDestroyed -= UpdateDestroyed;
                EnemyMain.EnemyDamageTaken -= UpdateGivenDamage;
            }
        }

        //
        // update info

        static void UpdateMovingDistance(object obj, float delta)
        {
            if (SV_Round.RoundNumber == 0) { return; }
            HistoryList.Last().valueList[HistoryValue.movingDistance] += delta;
        }

        static void UpdateTakeDamage(object obj, int[] damage)
        {
            if (SV_Round.RoundNumber == 0) { return; }
            HistoryList.Last().valueList[HistoryValue.takenDamage] += damage[0];

            HistoryUpdated?.Invoke(null, false);
        }

        static void UpdateGivenDamage(object obj, float damage)
        {
            if (SV_Round.RoundNumber == 0) { return; }
            HistoryList.Last().valueList[HistoryValue.givenDamage] += damage;

            HistoryUpdated?.Invoke(null, false);
        }

        static void UpdateGotMoney(object obj, int money)
        {
            if (SV_Round.RoundNumber == 0) { return; }
            HistoryList.Last().valueList[HistoryValue.gotMoney] += money;

            HistoryUpdated?.Invoke(null, false);
        }

        static void UpdateShotAmmo(object obj, Vector3 direction)
        {
            if (SV_Round.RoundNumber == 0) { return; }
            HistoryList.Last().valueList[HistoryValue.shotAmmo]++;

            HistoryUpdated?.Invoke(null, false);
        }

        static void UpdateDestroyed(object obj, EnemyMain main)
        {
            if (SV_Round.RoundNumber == 0) { return; }
            HistoryList.Last().valueList[HistoryValue.destroyed]++;

            HistoryUpdated?.Invoke(null, false);
        }

        static public void UpdateBuyList(Dictionary<ShopItem, int> buyList)
        {
            HistoryList.Last().buyList = new Dictionary<ShopItem, int>(buyList);
            HistoryUpdated?.Invoke(null, false);
        }

        //
        // show history

        static void ShowHistroyOnDead(object obj, bool mute)
        {
            CurrentCondition = Condition.dead;
            ShowHistory();
        }

        static void ShowHistoryOnTimeup(object obj, bool mute)
        {
            CurrentCondition = Condition.timeup;
            ShowHistory();
        }

        static public void ShowHistoryOnClear()
        {
            CurrentCondition = Condition.clear;
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

