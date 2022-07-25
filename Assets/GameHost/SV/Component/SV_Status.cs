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
        static public readonly int defaultMovingSpeedRate = 100;
        static public readonly int defaultWeaponSpeedRate = 100;
        static public readonly int defaultFiringSpeedRate = 100;

        static public EventHandler<int> PlayerDamageTaken { get; set; }
        static public EventHandler<bool> PlayerDead { get; set; }
        static public EventHandler<int> PlayerGotMoney { get; set; }

        static public int CurrentHP { get; private set; } = defaultMaxHP;
        static public int CurrentArmor { get; private set; } = defaultMaxArmor;
        static public int CurrentMoney { get; private set; } = 0;

        static public int CurrentMaxHP { get; private set; } = defaultMaxHP;
        static public int CurrentMaxArmor { get; private set; } = defaultMaxArmor;

        static public int CurrentDamageRate { get; private set; } = defaultDamageRate;
        static public int CurrentMoneyRate { get; private set; } = defaultMomeyRate;

        static public int MovingSpeedRate { get; private set; } = defaultMovingSpeedRate;
        static public int WeaponSpeedRate { get; private set; } = defaultWeaponSpeedRate;
        static public int FiringSpeedRate { get; private set; } = defaultFiringSpeedRate;

        public override void Initialize()
        {
            SetEvent(1);

            CurrentHP = defaultMaxHP;
            CurrentArmor = defaultMaxArmor;
            CurrentMoney = SvParams.GetInt(SvParam.initial_money);

            // set values
            CurrentMaxHP = defaultMaxHP;
            CurrentMaxArmor = defaultMaxArmor;
            CurrentDamageRate = defaultDamageRate;
            CurrentMoneyRate = defaultMomeyRate;

            MovingSpeedRate = defaultMovingSpeedRate;
            WeaponSpeedRate = defaultWeaponSpeedRate;
            FiringSpeedRate = defaultFiringSpeedRate;
        }

        public override void Shutdown()
        {
            SetEvent(-1);

            CurrentHP = defaultMaxHP;
            CurrentArmor = defaultMaxArmor;
            CurrentMoney = 0;

            // set values
            CurrentMaxHP = defaultMaxHP;
            CurrentMaxArmor = defaultMaxArmor;
            CurrentDamageRate = defaultDamageRate;
            CurrentMoneyRate = defaultMomeyRate;

            MovingSpeedRate = defaultMovingSpeedRate;
            WeaponSpeedRate = defaultWeaponSpeedRate;
            FiringSpeedRate = defaultFiringSpeedRate;
        }

        public override void Begin()
        {
            if (SV_Round.RoundNumber == 0) { return; }

            var rate = 1.0f + SvParams.Get(SvParam.money_increase_after_round);

            var moneyNext = CurrentMoney * rate;
            CurrentMoney = Mathf.RoundToInt(moneyNext);
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
            var armorDamage = Mathf.RoundToInt(gotDamage * Const.armor_reduction_rate);

            if (hpDamage < 1) { hpDamage = 1; }
            if (armorDamage < 1) { armorDamage = 1; }

            // armor damage
            CurrentArmor -= armorDamage;
            if (CurrentArmor < 0) { CurrentArmor = 0; }

            // hp damage
            CurrentHP -= hpDamage;
            PlayerDamageTaken?.Invoke(null, hpDamage);

            // is dead or not
            if (CurrentHP <= 0)
            {
                CurrentHP = 0;
                PlayerDead?.Invoke(null, false);
            }

            // - inner function
            static float DamageRate()
            {
                var constant = Const.damage_reduction_const;
                var armor = (float)CurrentArmor;

                return Calcf.SafetyDiv(constant, armor + constant, 1.0f);
            }
        }

        static public void Heal(int addHP)
        {
            var nextHP = CurrentHP + addHP;

            if (nextHP > CurrentMaxHP)
            {
                nextHP = CurrentMaxHP;
            }

            CurrentHP = nextHP;
        }

        static public void RepairArmor(int amount)
        {
            var nextArmor = CurrentArmor + amount;

            if (nextArmor > CurrentMaxArmor)
            {
                nextArmor = CurrentMaxArmor;
            }

            CurrentArmor = nextArmor;
        }

        static void ReceiveReward(object obj, EnemyMain enemyMain)
        {
            if (enemyMain.HP > 0) { return; }

            var reward = 0.0f;

            if (enemyMain.EnemyType == EnemyType.mine)
            {
                reward = SvParams.Get(SvParam.mine_destroy_reward);
            }

            if (enemyMain.EnemyType == EnemyType.turret)
            {
                reward = SvParams.Get(SvParam.turret_destroy_reward);
            }

            AddMoney(Mathf.RoundToInt(reward));
        }

        static public void AddMoney(int add)
        {
            var rate = Calcf.SafetyDiv(CurrentMoneyRate, defaultMomeyRate, 1.0f);
            var addMoney = Mathf.RoundToInt((float)add * rate);

            CurrentMoney += addMoney;
            PlayerGotMoney?.Invoke(null, addMoney);
        }

        static public float CurrentAkDamage()
        {
            var akDamage = SvParams.Get(SvParam.ak_damage);
            return akDamage * (float)CurrentDamageRate / (float)defaultDamageRate;
        }

        static public float CurrentDeDamage()
        {
            var deDamage = SvParams.Get(SvParam.de_damage);
            return deDamage * (float)CurrentDamageRate / (float)defaultDamageRate;
        }

        static public float MovingSpeed()
        {
            return (float)MovingSpeedRate / (float)defaultMovingSpeedRate;
        }

        static public float WeaponSpeed()
        {
            return (float)WeaponSpeedRate / (float)defaultWeaponSpeedRate;
        }

        static public float FiringSpeed()
        {
            return (float)FiringSpeedRate / (float)defaultFiringSpeedRate;
        }

        //
        // set method

        static public void SetArmor(int armor)
        {
            CurrentArmor = armor;
        }

        static public void SetHP(int hp)
        {
            CurrentHP = hp;
        }

        static public void SetMoney(int money)
        {
            CurrentMoney = money;
        }

        static public void SetMaxHP(int maxHP)
        {
            CurrentMaxHP = maxHP;
        }

        static public void SetMaxArmor(int maxArmor)
        {
            CurrentMaxArmor = maxArmor;
        }

        static public void SetDamageRate(int damageRate)
        {
            CurrentDamageRate = damageRate;
        }

        static public void SetMoneyRate(int moneyRate)
        {
            CurrentMoneyRate = moneyRate;
        }

        static public void SetMovingSpeedRate (int speedRate)
        {
            MovingSpeedRate = speedRate;
        }

        static public void SetWeaponSpeedRate(int speedRate)
        {
            WeaponSpeedRate = speedRate;
        }

        static public void SetFiringSpeedRate(int speedRate)
        {
            FiringSpeedRate = speedRate;
        }
    }
}

