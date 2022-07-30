using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum EnemyType
    {
        mine,
        turret,
    }

    public enum Killer
    {
        player,
        akm,
        deagle,
        myself,
    }

    public abstract class EnemyMain : MonoBehaviour
    {
        static public EventHandler<EnemyMain> EnemyDestroyed { get; set; }
        static public EventHandler<float> EnemyDamageTaken { get; set; }
        static public EventHandler<float> EnemyGivenDamage { get; set; }

        public EnemyType EnemyType { get; protected set; }
        public float HP { get; protected set; }
        public List<float> DamageHistory { get; protected set; }

        public EnemyBrain brain;

        protected void Init(EnemyType enemyType, InteractiveObject.OnShotReaction onShot)
        {
            EnemyType = enemyType;

            if (enemyType == EnemyType.mine)
            {
                var defaultHP = SvParams.GetInt(SvParam.mine_hp);
                var rate = SvParams.Get(SvParam.mine_hp_increase);

                HP = defaultHP * (1.0f + rate * SV_Round.RoundNumber);
            }

            else
            {
                var defaultHP = SvParams.GetInt(SvParam.turret_hp);
                var rate = SvParams.Get(SvParam.turret_hp_increase);

                HP = defaultHP * (1.0f + rate * SV_Round.RoundNumber);
            }

            var interactive = gameObject.GetComponent<InteractiveObject>();
            interactive.SetOnShotReaction(onShot);

            brain = gameObject.GetComponent<EnemyBrain>();
            DamageHistory = new List<float>();
        }

        protected virtual void OnShot()
        {
            var distance = (Player.Myself.transform.position - gameObject.transform.position).magnitude;
            var damage = GetDamage(distance);

            DamageHistory.Add(damage);

            if (damage < HP)
            {
                HP -= damage;
                EnemyDamageTaken?.Invoke(null, damage);
            }

            else
            {
                EnemyDamageTaken?.Invoke(null, HP);
                HP = 0.0f;

                EnemyDestroyed?.Invoke(null, this);
                Destroy(gameObject);
                return;
            }

            ForceDetection();
        }

        static protected float GetDamage(float distance)
        {
            var weapon = WeaponSystem.CurrentWeapon.Weapon;

            if (weapon == Weapon.ak)
            {
                var reduction = DistanceReduction(distance, Const.ak_non_reduction_distance, Const.ak_reduction_rate, Const.ak_min_reduction_rate);
                return SV_Status.CurrentAkDamage() * reduction;
            }

            if (weapon == Weapon.de)
            {
                var reduction = DistanceReduction(distance, Const.de_non_reduction_distance, Const.de_reduction_rate, Const.de_min_reduction_rate);
                return SV_Status.CurrentDeDamage();
            }

            return 0.0f;

            // - inner function
            static float DistanceReduction(float distance, float nonReductionDistance, float reductionRate, float minReductionRate)
            {
                if (distance < nonReductionDistance) { return 1.0f; }

                var offset = distance - nonReductionDistance;

                var rate = 1.0f - (reductionRate * 0.1f) * offset;
                rate = Calcf.Clip(minReductionRate, 1.0f, rate);

                return rate;
            }
        }

        void ForceDetection()
        {
            var distance = (Player.Myself.transform.position - gameObject.transform.position).magnitude;
            if (distance > Const.enemy_detect_range) { return; }

            brain.ForceDetection();
        }
    }
}

