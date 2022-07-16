using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class MineMain : EnemyMain
    {
        static public EventHandler<MineMain> Explosion { get; set; }

        private void Awake()
        {
            var _hp = Params.mine_hp;
            var rate = Params.mine_hp_increase;

            HP = _hp * (1.0f + rate * SV_RoundAdmin.RoundNumber);

            var interactive = gameObject.GetComponent<InteractiveObject>();
            interactive.SetOnShotReaction(OnShot);

            EnemyType = EnemyType.mine;
        }

        void OnShot()
        {
            var damageRate = Calcf.SafetyDiv((float)SV_StatusAdmin.CurrentDamageRate, (float)SV_StatusAdmin.DefaultDamageRate, 1.0f);
            var damage = GetDamage() * damageRate;

            HP -= damage;

            if (HP <= 0.0f)
            {
                KilledBy = Killer.player;
                EnemyDestroyed?.Invoke(this, this);
                Destroy(gameObject);
            }

            // - inner function
            static float GetDamage()
            {
                //var weapon = WeaponManager.ActiveWeapon;

                //if (weapon == Weapon.akm)
                //{
                //    return Params.ak_damage;
                //}

                //if (weapon == Weapon.deagle)
                //{
                //    return Params.de_damage;
                //}

                return 0.0f;
            }
        }

        public void OnTriggerStay(Collider collider)
        {
            if (collider.tag == "Player")
            {
                var _damage = Params.mine_damage;
                var rate = Params.mine_damage_increase;
                var damage = _damage + (1.0f + rate * SV_RoundAdmin.RoundNumber);

                KilledBy = Killer.myself;
                SV_StatusAdmin.DamageTaken?.Invoke(null, damage);
                Explosion?.Invoke(null, this);
                EnemyDestroyed?.Invoke(null, this);

                Destroy(gameObject);
                return;
            }
        }
    }
}

