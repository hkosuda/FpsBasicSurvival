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
        static public EventHandler<EnemyMain> EnemyDamageTaken { get; set; }
        static public EventHandler<EnemyMain> EnemyDestroyed { get; set; }

        public EnemyType EnemyType { get; protected set; }

        public Killer KilledBy { get; protected set; }
        public float HP { get; protected set; }

        public void OnTriggerEnter(Collider other)
        {
            //if (other.tag == "DropAKM" || other.tag == "DropDeagle")
            //{
            //    var dropWeapon = other.transform.GetChild(0).gameObject.GetComponent<DropWeapon>();

            //    if (!dropWeapon.Offensive) { return; }

            //    if (other.GetComponent<Rigidbody>().velocity.magnitude > 0.0f)
            //    {
            //        dropWeapon.Offensive = false;

            //        KilledBy = GetKiller(other.gameObject);
            //        HP = 0.0f;
            //        EnemyDestroyed?.Invoke(null, this);
            //        Destroy(gameObject);
            //    }
            //}

            //// - inner function
            //static Killer GetKiller(GameObject _gameObject)
            //{
            //    if (_gameObject.tag == "DropAKM")
            //    {
            //        return Killer.akm;
            //    }

            //    else
            //    {
            //        return Killer.deagle;
            //    }
            //}
        }
    }
}

