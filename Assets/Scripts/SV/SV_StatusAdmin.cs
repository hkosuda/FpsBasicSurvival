using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum SV_Status
    {
        hp,
        armor,
        money,
    }

    public class SV_StatusAdmin : HostComponent
    {
        static public EventHandler<float> DamageTaken { get; set; }
        static public EventHandler<float> Healing { get; set; }
        static public EventHandler<bool> Dead { get; set; }

        static public Dictionary<SV_Status, int> StatusList { get; private set; }

        static public int DefaultMaxHP { get; } = 1000;
        static public int DefaultMaxArmor { get; } = 1000;
        static public int DefaultDamageRate { get; } = 100;
        static public int DefaultMomeyRate { get; } = 100;

        static public int CurrentMaxHP { get; set; }
        static public int CurrentMaxArmor { get; set; }
        static public int CurrentDamageRate { get; set; }
        static public int CurrentMoneyRate { get; set; }

        public SV_StatusAdmin()
        {
            StatusList = new Dictionary<SV_Status, int>()
        {
            { SV_Status.hp, DefaultMaxHP - 200 }, { SV_Status.armor, DefaultMaxArmor }, { SV_Status.money, Params.initial_money },
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
            if (SV_RoundAdmin.RoundNumber == 0) { return; }

            var f_moneyNext = StatusList[SV_Status.money] * (1.0f + Params.money_increase_after_round);
            StatusList[SV_Status.money] = Mathf.RoundToInt(f_moneyNext);
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
            Debug.Log(StatusList[SV_Status.hp]);

            var hpDamage = getDamage * GetDamageRate();
            var armorDamage = getDamage * Params.sv_armor_reduction_rate;

            if (hpDamage < 1.0f) { hpDamage = 1.0f; }
            if (armorDamage < 1.0f) { armorDamage = 1.0f; }

            // processing
            StatusList[SV_Status.armor] -= Mathf.RoundToInt(armorDamage);
            if (Mathf.RoundToInt(StatusList[SV_Status.armor]) <= 0) { StatusList[SV_Status.armor] = 0; }

            // player dead
            StatusList[SV_Status.hp] -= Mathf.RoundToInt(hpDamage);
            if (Mathf.RoundToInt(StatusList[SV_Status.hp]) <= 0)
            {
                StatusList[SV_Status.hp] = 0;
                Dead?.Invoke(null, false);
            }

            // - inner function
            static float GetDamageRate()
            {
                var constant = Params.damage_reduction_const;
                var armor = StatusList[SV_Status.armor];

                return Calcf.SafetyDiv(constant, armor + constant, 1.0f);
            }
        }

        static void Heal(object obj, float heal)
        {
            var currentHP = StatusList[SV_Status.hp];
            var nextHP = currentHP + Mathf.RoundToInt(Mathf.Abs(heal));

            if (nextHP > CurrentMaxHP)
            {
                nextHP = CurrentMaxHP;
            }

            StatusList[SV_Status.hp] = nextHP;
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

            StatusList[SV_Status.money] += Mathf.RoundToInt(reward * rate);
        }
    }
}

