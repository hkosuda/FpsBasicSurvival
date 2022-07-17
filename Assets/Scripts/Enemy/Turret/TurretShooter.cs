using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TurretShooter : MonoBehaviour
    {
        public EventHandler<bool> Shot { get; set; }

        static float CannonHeight { get; } = 1.1f;

        float cooldownRemain = 1.0f;
        GameObject cannon;

        private void Awake()
        {
            cooldownRemain = 0.0f;
            cannon = gameObject.transform.GetChild(0).gameObject;
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

        void UpdateMethod(object obj, float dt)
        {
            cooldownRemain -= dt;

            if (cooldownRemain <= 0.0f)
            {
                cooldownRemain = -1.0f;
            }
        }

        public void Shoot()
        {
            if (cooldownRemain > 0.0f) { return; }

            ResetCooldownTime();

            var origin = new Vector3(gameObject.transform.position.x, CannonHeight, gameObject.transform.position.z);
            var direction = Shooter.GetDirectionWithSpread(origin, Player.Myself.transform.position);
            var distance = Params.turret_shell_speed * Params.turret_shell_exist_time;

            Physics.Raycast(origin, direction, out RaycastHit hit, distance);

            if (hit.collider == null) { return; }

            TurretShell.GenerateBullet(origin, direction);

            SetAngle();

            Shot?.Invoke(null, false);
        }

        void SetAngle()
        {
            var distance = (Player.Myself.transform.position - gameObject.transform.position).magnitude;
            var height = gameObject.transform.position.y - CannonHeight;

            var rotX = Mathf.Atan2(height, distance) * Mathf.Rad2Deg;

            cannon.transform.localRotation = Quaternion.Euler(-rotX, 0.0f, 0.0f);
        }

        public void ResetCooldownTime()
        {
            cooldownRemain = Params.turret_shooting_interval;
        }
    }

    public class Shooter
    {
        static public Vector3 GetDirectionWithSpread(Vector3 origin, Vector3 target)
        {
            var rotXY = GetRotation(origin, target);

            float rotX = rotXY[0];
            float rotY = rotXY[1];

            float[] pqr_vector = new float[3] { 10.0f, 0.0f, 0.0f };

            pqr_vector = CalcRandomSpread(pqr_vector);

            float[] zxy_vector = PQR2ZXY(pqr_vector, rotX, rotY);

            float z = zxy_vector[0];
            float x = zxy_vector[1];
            float y = zxy_vector[2];

            var spread = new Vector3(x, y, z);
            var direction = spread.normalized;



            return direction;

            // function
            static float[] GetRotation(Vector3 origin, Vector3 target)
            {
                var dz = target.z - origin.z;
                var dx = target.x - origin.x;
                var dy = target.y - origin.y;

                var dL = Mathf.Sqrt(Mathf.Pow(dz, 2.0f) + Mathf.Pow(dx, 2.0f));

                var rotY = Mathf.Atan2(dx, dz);
                var rotX = Mathf.Atan2(dy, dL);

                return new float[2] { rotX, rotY };
            }

            // function
            static float[] CalcRandomSpread(float[] pqr_vector)
            {
                float p = pqr_vector[0];
                float q = pqr_vector[1];
                float r = pqr_vector[2];

                // <calc spread>
                float h_max = Params.turret_shell_h_spread;
                float v_max = Params.turret_shell_v_spread;

                float[] qr_spread = GetEllipseRandomSpread(h_max, v_max);
                // </calc spread>

                // <add spread>
                q += qr_spread[0];
                r += qr_spread[1];
                // </add spread>

                return new float[3] { p, q, r };
            }

            // function
            static float[] GetEllipseRandomSpread(float h_max, float v_max)
            {
                if (h_max == 0.0f)
                {
                    return new float[2] { 0.0f, 0.0f };
                }

                float qq = UnityEngine.Random.Range(-h_max, h_max);

                float rr_max = v_max * Mathf.Sqrt(1 - Mathf.Pow(qq / h_max, 2.0f));

                float rr = UnityEngine.Random.Range(-rr_max, rr_max);

                return new float[2] { qq, rr };
            }

            static float[] PQR2ZXY(float[] pqr_vector, float rotX, float rotY)
            {
                float p = pqr_vector[0];
                float q = pqr_vector[1];
                float r = pqr_vector[2];

                float sX = Mathf.Sin(rotX);
                float cX = Mathf.Cos(rotX);
                float sY = Mathf.Sin(rotY);
                float cY = Mathf.Cos(rotY);

                float z = p * cX * cY - q * sY - r * sX * cY;
                float x = p * cX * sY + q * cY - r * sX * sY;
                float y = p * sX + r * cX;

                return new float[3] { z, x, y };
            }
        }
    }
}

