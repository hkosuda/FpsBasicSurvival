using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [RequireComponent(typeof(TurretMain))]
    public class TurretRagdollSystem : MonoBehaviour
    {
        static readonly float akForceMin = 10000.0f;
        static readonly float akForceMax = 15000.0f;

        static readonly float akSpeedMin = 5.0f;
        static readonly float akSpeedMax = 8.0f;

        static readonly float deForceMin = 20000.0f;
        static readonly float deForceMax = 30000.0f;

        static readonly float deSpeedMin = 8.0f;
        static readonly float deSpeedMax = 12.0f;

        static GameObject _turretRagdoll;

        TurretMain main;

        Vector3 direction;
        Vector3 hitPoint;

        private void Awake()
        {
            if (_turretRagdoll == null) { _turretRagdoll = Resources.Load<GameObject>("SV/TurretRagdoll"); }
            main = gameObject.GetComponent<TurretMain>();
        }

        private void Start()
        {
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        private void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                WeaponController.Shot += CheckDirection;
                WeaponController.ShootingHit += CheckHitpoint;

                EnemyMain.EnemyDestroyed += InstantiateRagdoll;
            }

            else
            {
                WeaponController.Shot -= CheckDirection;
                WeaponController.ShootingHit -= CheckHitpoint;

                EnemyMain.EnemyDestroyed -= InstantiateRagdoll;
            }
        }

        void CheckDirection(object obj, Vector3 _direction)
        {
            direction = _direction;
        }

        void CheckHitpoint(object obj, RaycastHit hit)
        {
            hitPoint = hit.point;
        }

        void InstantiateRagdoll(object obj, EnemyMain _main)
        {
            if (main != _main) { return; }

            var ragdoll = GameHost.Instantiate(_turretRagdoll);
            ragdoll.transform.position = gameObject.transform.position;
            ragdoll.transform.eulerAngles = gameObject.transform.eulerAngles;

            Vector3 force;
            Vector3 velocity;

            if (WeaponSystem.CurrentWeapon.Weapon == Weapon.ak)
            {
                force = direction.normalized * UnityEngine.Random.Range(akForceMin, akForceMax);
                velocity = direction.normalized * UnityEngine.Random.Range(akSpeedMin, akSpeedMax);
            }

            else
            {
                force = direction.normalized * UnityEngine.Random.Range(deForceMin, deForceMax);
                velocity = direction.normalized * UnityEngine.Random.Range(deSpeedMin, deSpeedMax);
            }

            var rb = ragdoll.GetComponent<Rigidbody>();

            rb.velocity = velocity;
            rb.AddForceAtPosition(force, hitPoint);
        }
    }
}
