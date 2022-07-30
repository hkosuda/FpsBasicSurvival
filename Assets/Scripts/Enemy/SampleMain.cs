using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(SampleMovingSystem))]
    public class SampleMain : MonoBehaviour
    {
        static readonly int maxHit = 4;

        int hit = 0;
        SampleMovingSystem movingSystem;

        private void Awake()
        {
            hit = maxHit;
            movingSystem = gameObject.GetComponent<SampleMovingSystem>();
        }

        private void Start()
        {
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                WeaponController.ShootingHit += CalcDamage;
            }

            else
            {
                WeaponController.ShootingHit -= CalcDamage;
            }
        }

        void CalcDamage(object obj, RaycastHit hitInfo)
        {
            if (hitInfo.collider.gameObject == gameObject)
            {
                hit--;
                EnemyMain.EnemyDamageTaken?.Invoke(null, 0.0f);

                if (hit <= 0)
                {
                    hit = maxHit;
                    movingSystem.RandomSpawn();
                }
            }
        }
    }
}

