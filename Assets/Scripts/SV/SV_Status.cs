using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum Status
    {
        hp,
        armor,
        money,
    }

    public class SV_Status : HostComponent
    {
        static public EventHandler<float> DamageTaken { get; set; }
        static public EventHandler<float> Healing { get; set; }
        static public EventHandler<bool> Dead { get; set; }

        static public Dictionary<Status, int> StatusList { get; private set; }

        static public int DefaultMaxHP { get; } = 1000;
        static public int DefaultMaxArmor { get; } = 1000;
        static public int DefaultDamageRate { get; } = 100;
        static public int DefaultMomeyRate { get; } = 100;

        static public int CurrentMaxHP { get; set; }
        static public int CurrentMaxArmor { get; set; }
        static public int CurrentDamageRate { get; set; }
        static public int CurrentMoneyRate { get; set; }

        public override void Initialize()
        {
            StatusList = new Dictionary<Status, int>()
            {
                { Status.hp, DefaultMaxHP - 200 }, { Status.armor, DefaultMaxArmor }, { Status.money, Params.initial_money },
            };

            // set values
            CurrentMaxHP = DefaultMaxHP;
            CurrentMaxArmor = DefaultMaxArmor;
            CurrentDamageRate = DefaultDamageRate;
            CurrentMoneyRate = DefaultMomeyRate;

            SetEvent(1);
        }

        public override void Begin()
        {
            if (SV_Round.RoundNumber == 0) { return; }

            var f_moneyNext = StatusList[Status.money] * (1.0f + Params.money_increase_after_round);
            StatusList[Status.money] = Mathf.RoundToInt(f_moneyNext);
        }

        public override void Stop()
        {
            
        }

        public override void Shutdown()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                DamageTaken += TakeDamage;
                Healing += Heal;

                EnemyMain.EnemyDestroyed += ReceiveReward;
            }

            else
            {
                DamageTaken -= TakeDamage;
                Healing -= Heal;

                EnemyMain.EnemyDestroyed -= ReceiveReward;
            }
        }

        static public void RoundIncrement()
        {

        }

        static void TakeDamage(object obj, float getDamage)
        {
            Debug.Log(StatusList[Status.hp]);

            var hpDamage = getDamage * GetDamageRate();
            var armorDamage = getDamage * Params.sv_armor_reduction_rate;

            if (hpDamage < 1.0f) { hpDamage = 1.0f; }
            if (armorDamage < 1.0f) { armorDamage = 1.0f; }

            // processing
            StatusList[Status.armor] -= Mathf.RoundToInt(armorDamage);
            if (Mathf.RoundToInt(StatusList[Status.armor]) <= 0) { StatusList[Status.armor] = 0; }

            // player dead
            StatusList[Status.hp] -= Mathf.RoundToInt(hpDamage);
            if (Mathf.RoundToInt(StatusList[Status.hp]) <= 0)
            {
                StatusList[Status.hp] = 0;
                Dead?.Invoke(null, false);
            }

            // - inner function
            static float GetDamageRate()
            {
                var constant = Params.damage_reduction_const;
                var armor = StatusList[Status.armor];

                return Calcf.SafetyDiv(constant, armor + constant, 1.0f);
            }
        }

        static void Heal(object obj, float heal)
        {
            var currentHP = StatusList[Status.hp];
            var nextHP = currentHP + Mathf.RoundToInt(Mathf.Abs(heal));

            if (nextHP > CurrentMaxHP)
            {
                nextHP = CurrentMaxHP;
            }

            StatusList[Status.hp] = nextHP;
        }

        static void ReceiveReward(object obj, EnemyMain enemyMain)
        {
            if (enemyMain.HP > 0) { return; }

            var rate = Calcf.SafetyDiv(CurrentMoneyRate, DefaultMomeyRate, 0.0f);
            var reward = 0.0f;

            if (enemyMain.EnemyType == EnemyType.mine)
            {
                reward = Params.mine_destroy_reward;
            }

            if (enemyMain.EnemyType == EnemyType.turret)
            {
                reward = Params.turret_destroy_reward;
            }

            StatusList[Status.money] += Mathf.RoundToInt(reward * rate);
        }
    }
}

