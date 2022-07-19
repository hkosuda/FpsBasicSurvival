using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class MineMain : EnemyMain
    {
        static public EventHandler<MineMain> MineExplosion { get; set; }

        private void Awake()
        {
            Init(EnemyType.mine, OnShot);
        }

        public void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.layer == Const.playerLayer)
            {
                var defaultDamage = Params.mine_damage;
                var rate = Params.mine_damage_increase;
                var damage = defaultDamage * (1.0f + rate * SV_Round.RoundNumber);

                EnemyGivenDamage?.Invoke(null, damage);
                MineExplosion?.Invoke(null, this);

                Destroy(gameObject);
                return;
            }
        }
    }
}

