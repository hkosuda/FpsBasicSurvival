using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame
{
    public class BulletLine : MonoBehaviour
    {
        static readonly float bulletSpeed = 350.0f;
        static readonly float bulletMaxExistTime = 3.0f;

        float pastTime;
        float bulletExistTime;

        Vector3 direction;
        Vector3 origin;

        GameObject body;

        private void Awake()
        {
            pastTime = 0.0f;
            body = gameObject.transform.GetChild(0).gameObject;
        }


        void Start()
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
                TimerSystem.Updated += UpdateMethod;
                TimerSystem.TimerPaused += HideBullets;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
                TimerSystem.TimerPaused -= HideBullets;
            }
        }

        void UpdateMethod(object obj, float dt)
        {
            pastTime += dt;
            if (pastTime > bulletExistTime) { Destroy(gameObject); return; }

            gameObject.transform.position = origin + direction * bulletSpeed * pastTime;
        }

        void HideBullets(object obj, bool mute)
        {
            if (body != null)
            {
                body.SetActive(false);
            }
        }

        public void SetExistTime(float existTime)
        {
            bulletExistTime = existTime;
        }

        public void Setup(Vector3 direction, Vector3 origin)
        {
            this.direction = direction;
            this.origin = origin;
        }

        //
        // Static Members

        static GameObject _bullet;
        static GameObject muzzle;

        static BulletLine bullet;

        static public void Initialize()
        {
            _bullet = Resources.Load<GameObject>("Object/BulletLine");
            muzzle = GameObject.FindWithTag("Muzzle");

            _SetEvent(1);
        }

        static public void Shutdown()
        {
            _bullet = null;
            _SetEvent(-1);
        }

        static void _SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                WeaponController.Shot += StartPreview;
                WeaponController.ShootingHit += UpdateExistTime;
            }

            else
            {
                WeaponController.Shot -= StartPreview;
                WeaponController.ShootingHit -= UpdateExistTime;
            }
        }

        static void StartPreview(object obj, Vector3 _direction)
        {
            if (_direction.magnitude == 0.0f) { return; }

            var direction = _direction.normalized;
            var origin = muzzle.transform.position;

            var initialPos = origin + _direction * UnityEngine.Random.Range(0.5f, 2.0f);
            var bulletLine = GameHost.Instantiate(_bullet, initialPos, Quaternion.identity);

            bullet = bulletLine.GetComponent<BulletLine>();
            bullet.Setup(direction, origin);
            bullet.SetExistTime(bulletMaxExistTime);
            
            SetRotation(bulletLine, direction);

            // - inner function
            static void SetRotation(GameObject bullet, Vector3 direction)
            {
                var rotY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                var rotX = -Mathf.Atan2(direction.y, new Vector2(direction.x, direction.z).magnitude) * Mathf.Rad2Deg;

                bullet.transform.eulerAngles = new Vector3(rotX, rotY, 0.0f);
            }
        }

        static void UpdateExistTime(object obj, RaycastHit hit)
        {
            if (bullet != null)
            {
                var existTime = Calcf.SafetyDiv((hit.point - muzzle.transform.position).magnitude, bulletSpeed, 0.0f);
                bullet.SetExistTime(existTime);
            }
        }
    }
}

