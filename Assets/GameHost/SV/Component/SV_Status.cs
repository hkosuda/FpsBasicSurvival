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
        static public readonly int defaultMaxHP = 1000;
        static public readonly int defaultMaxArmor = 1000;
        static public readonly int defaultDamageRate = 100;
        static public readonly int defaultMomeyRate = 100;

        static public EventHandler<int> PlayerDamageTaken { get; set; }
        static public EventHandler<bool> PlayerDead { get; set; }
        static public EventHandler<int> PlayerGotMoney { get; set; }

        static public Dictionary<Status, int> StatusList { get; private set; }

        static public int CurrentMaxHP { get; set; }
        static public int CurrentMaxArmor { get; set; }
        static public int CurrentDamageRate { get; set; }
        static public int CurrentMoneyRate { get; set; }

        public override void Initialize()
        {
            StatusList = new Dictionary<Status, int>()
            {
                { Status.hp, defaultMaxHP }, { Status.armor, defaultMaxArmor }, { Status.money, Params.initial_money },
            };

            // set values
            CurrentMaxHP = defaultMaxHP;
            CurrentMaxArmor = defaultMaxArmor;
            CurrentDamageRate = defaultDamageRate;
            CurrentMoneyRate = defaultMomeyRate;

            SetEvent(1);
        }

        public override void Shutdown()
        {
            SetEvent(-1);
        }

        public override void Begin()
        {
            if (SV_Round.RoundNumber == 0) { return; }

            var moneyNext = StatusList[Status.money] * (1.0f + Params.money_increase_after_round);
            StatusList[Status.money] = Mathf.RoundToInt(moneyNext);
        }

        public override void Stop()
        {
            
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                EnemyMain.EnemyGivenDamage += TakeDamage;
                EnemyMain.EnemyDestroyed += ReceiveReward;
            }

            else
            {
                EnemyMain.EnemyGivenDamage -= TakeDamage;
                EnemyMain.EnemyDestroyed -= ReceiveReward;
            }
        }

        static void TakeDamage(object obj, float gotDamage)
        {
            var hpDamage = Mathf.RoundToInt(gotDamage * DamageRate());
            var armorDamage = Mathf.RoundToInt(gotDamage * Params.sv_armor_reduction_rate);

            if (hpDamage < 1) { hpDamage = 1; }
            if (armorDamage < 1) { armorDamage = 1; }

            // armor damage
            StatusList[Status.armor] -= armorDamage;
            if (Mathf.RoundToInt(StatusList[Status.armor]) < 0) { StatusList[Status.armor] = 0; }

            // hp damage
            StatusList[Status.hp] -= hpDamage;
            PlayerDamageTaken?.Invoke(null, hpDamage);

            // is dead or not
            if (Mathf.RoundToInt(StatusList[Status.hp]) <= 0)
            {
                StatusList[Status.hp] = 0;
                PlayerDead?.Invoke(null, false);
            }

            // - inner function
            static float DamageRate()
            {
                var constant = Params.damage_reduction_const;
                var armor = (float)StatusList[Status.armor];

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

            var rate = Calcf.SafetyDiv(CurrentMoneyRate, defaultMomeyRate, 0.0f);
            var reward = 0.0f;

            if (enemyMain.EnemyType == EnemyType.mine)
            {
                reward = Params.mine_destroy_reward;
            }

            if (enemyMain.EnemyType == EnemyType.turret)
            {
                reward = Params.turret_destroy_reward;
            }

            AddMoney(Mathf.RoundToInt(reward * rate));
        }

        static public void AddMoney(int add)
        {
            StatusList[Status.money] += add;
            PlayerGotMoney?.Invoke(null, add);
        }

        static public float CurrentAkDamage()
        {
            var akDamage = Params.ak_damage;
            return akDamage * (float)CurrentDamageRate / (float)defaultDamageRate;
        }

        static public float CurrentDeDamage()
        {
            var deDamage = Params.de_damage;
            return deDamage * (float)CurrentDamageRate / (float)defaultDamageRate;
        }
    }
}

