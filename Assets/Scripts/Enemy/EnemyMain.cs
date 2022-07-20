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
        static readonly Dictionary<EnemyType, int> defaultHpList = new Dictionary<EnemyType, int>()
        {
            {EnemyType.mine, 20 }, { EnemyType.turret, 40 }
        };

        static readonly Dictionary<EnemyType, int> hpIncreaseList = new Dictionary<EnemyType, int>()
        {
            {EnemyType.mine, 1 }, { EnemyType.turret, 1 }
        };

        static public EventHandler<EnemyMain> EnemyDamageTaken { get; set; }
        static public EventHandler<EnemyMain> EnemyDestroyed { get; set; }
        static public EventHandler<float> EnemyGivenDamage { get; set; }

        public EnemyType EnemyType { get; protected set; }
        public float HP { get; protected set; }

        protected void Init(EnemyType enemyType, InteractiveObject.OnShotReaction onShot)
        {
            EnemyType = enemyType;

            var defaultHP = defaultHpList[enemyType];
            var rate = hpIncreaseList[enemyType];

            HP = defaultHP * (1.0f + rate * SV_Round.RoundNumber);

            var interactive = gameObject.GetComponent<InteractiveObject>();
            interactive.SetOnShotReaction(onShot);
        }

        protected virtual void OnShot()
        {
            var damage = GetDamage();

            HP -= damage;
            EnemyDamageTaken?.Invoke(null, this);

            if (HP <= 0.0f)
            {
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

