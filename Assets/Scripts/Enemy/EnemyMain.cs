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
        }

        protected virtual void OnShot()
        {
            var damage = GetDamage();

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
            }
        }

        static protected float GetDamage()
        {
            var weapon = WeaponSystem.CurrentWeapon.Weapon;

            if (weapon == Weapon.ak)
            {
                return SV_Status.CurrentAkDamage();
            }

            if (weapon == Weapon.de)
            {
                return SV_Status.CurrentDeDamage();
            }

            return 0.0f;
        }
    }
}

