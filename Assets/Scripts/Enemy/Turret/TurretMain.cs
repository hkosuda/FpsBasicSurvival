using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TurretMain : EnemyMain
    {
        private void Awake()
        {
            var _hp = Params.turret_hp;
            var rate = Params.turret_hp_increase;

            HP = _hp * (1.0f + rate * SV_RoundAdmin.RoundNumber);

            var interactive = gameObject.GetComponent<InteractiveObject>();
            interactive.SetOnShotReaction(OnShot);

            EnemyType = EnemyType.turret;
        }

        void OnShot()
        {
            var damageRate = Calcf.SafetyDiv((float)SV_StatusAdmin.CurrentDamageRate, (float)SV_StatusAdmin.DefaultDamageRate, 1.0f);
            var damage = Params.ak_damage * damageRate;

            HP -= damage;
            EnemyDamageTaken(null, this);

            if (HP <= 0.0f)
            {
                KilledBy = Killer.player;
                EnemyDestroyed?.Invoke(this, this);
                Destroy(gameObject);
            }
        }
    }
}

