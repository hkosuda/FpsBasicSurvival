using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class KnifeController : MonoBehaviour
    {
        static public EventHandler<RaycastHit> Shot { get; set; }
        static public EventHandler<GameObject> ShootingHit { get; set; }

        static public float CooldownRemain { get; private set; }

        static public void Initialize()
        {

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

        static void UpdateMethod(object obj, float dt)
        {
            CooldownRemain = Calcf.Clip(CooldownRemain - dt, 0.0f, 0.1f);

            if (Keyconfig.CheckInput(Keyconfig.KeyAction.shot, true))
            {
                if (CheckAvailability())
                {
                    SemiAuto();
                }
            }

            //
            // - inner function
            static bool CheckAvailability()
            {
                if (!WeaponManager.Active) { return false; }
                if (CooldownRemain > 0.0f) { return false; }

                return true;
            }
        }

        static void SemiAuto()
        {
            if (!WeaponManager.Active) { return; }
            if (CooldownRemain > 0.0f) { return; }

            var hit = GetHit();

            Shot?.Invoke(null, hit);

            if (hit.collider != null)
            {
                ShootingHit?.Invoke(null, hit.collider.gameObject);
            }

            // - inner function
            static RaycastHit GetHit()
            {
                var camera = Camera.main;

                var ray = camera.ScreenPointToRay(Input.mousePosition);
                var distance = 1.5f;

                Physics.Raycast(ray: ray, out RaycastHit hit, maxDistance: distance, ~(1 << 2));

                return hit;
            }

            // - inner function
            static void UpdateStatus()
            {
                CooldownRemain = 1.0f;
            }
        }
    }
}

