using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TurretShell : MonoBehaviour
    {
        float pastTime;

        Vector3 direction;
        Vector3 origin;

        private void Awake()
        {
            pastTime = 0.0f;
            origin = gameObject.transform.position;
            gameObject.transform.SetParent(GameHost.World.transform);
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
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
            }
        }

        // Update is called once per frame
        void UpdateMethod(object obj, float dt)
        {
            pastTime += Time.deltaTime;
            if (pastTime > Params.turret_shell_exist_time) { Destroy(gameObject); }

            var speed = Params.turret_shell_speed;
            gameObject.transform.position = origin + direction.normalized * pastTime * speed;

            RaycastCheck(gameObject.transform.position, direction, speed * dt);
        }

        public void SetDirection(Vector3 direction)
        {
            this.direction = direction;
        }

        private void OnTriggerStay(Collider other)
        {
            string tag = other.gameObject.tag;

            if (tag == "Turret")
            {
                return;
            }

            if (tag == "Player")
            {
                DamageProcessing();
            }

            else
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            var tag = collision.gameObject.tag;

            if (tag == "Turret")
            {
                return;
            }

            Destroy(gameObject);
        }

        void DamageProcessing()
        {
            var _damage = Params.turret_damage;
            var rate = Params.turret_damage_increase;

            var damage = _damage * (1.0f + rate * SV_RoundAdmin.RoundNumber);

            SV_StatusAdmin.DamageTaken?.Invoke(null, damage);
            Destroy(gameObject);
        }

        void RaycastCheck(Vector3 origin, Vector3 direction, float distance)
        {
            Physics.SphereCast(origin: origin, radius: 0.05f, direction: direction, out RaycastHit hit, maxDistance: distance);

            if (hit.collider == null)
            {
                return;
            }

            if (hit.collider.tag == "Player")
            {
                DamageProcessing();
            }

            else
            {
                Destroy(gameObject);
            }
        }

        //
        // static member and methods

        static GameObject _shell;

        static public void Shutdown()
        {
            _shell = null;
        }

        static public void GenerateBullet(Vector3 origin, Vector3 direction)
        {
            if (_shell == null)
            {
                _shell = Resources.Load<GameObject>("Enemy/TurretShell");
            }

            var bullet = Object.Instantiate(_shell, origin, Quaternion.identity);

            bullet.transform.SetParent(GameHost.World.transform);
            bullet.GetComponent<TurretShell>().SetDirection(direction.normalized);
        }
    }
}

