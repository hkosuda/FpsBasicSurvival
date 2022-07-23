using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class WeaponAnimator : MonoBehaviour
    {
        static readonly float switchIntervalThresh = 2.0f;
        static readonly float crowbarProbability = 0.05f;

        public enum AnimationWeapon
        {
            ak, de, m9, bar,
        }

        static public AnimationWeapon CurrentWeapon { get; private set; }

        static Animator animator;
        static float switchInterval;

        void Awake()
        {
            animator = GetComponent<Animator>();
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                TimerSystem.Updated += UpdateMethod;

                WeaponSystem.WeaponChanged += BeginTakingoutAnimation;
                WeaponController.Shot += BeginShotAnimation;

                AK_Reload.ReloadingBegin += BeginReloadingAnimation;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;

                WeaponSystem.WeaponChanged -= BeginTakingoutAnimation;
                WeaponController.Shot -= BeginShotAnimation;

                AK_Reload.ReloadingBegin -= BeginReloadingAnimation;
            }
        }
        
        static void UpdateMethod(object obj, float dt)
        {
            switchInterval += dt;
            if (switchInterval > switchIntervalThresh) { switchInterval = switchIntervalThresh + 1.0f; }
        }

        static void BeginTakingoutAnimation(object obj, Weapon weapon)
        {
            animator.speed = SV_Status.WeaponSpeed();

            if (weapon == Weapon.ak)
            {
                animator.SetTrigger("SwitchAk");
                CurrentWeapon = AnimationWeapon.ak;
            }

            else if (weapon == Weapon.de)
            {
                animator.SetTrigger("SwitchDe");
                CurrentWeapon = AnimationWeapon.de;
            }

            else if (weapon == Weapon.m9)
            {
                animator.speed = 1.0f;

                if (switchInterval < switchIntervalThresh)
                {
                    animator.SetTrigger("SwitchM9");
                    CurrentWeapon = AnimationWeapon.m9;
                }

                else
                {
                    var rnd = UnityEngine.Random.Range(0.0f, 1.0f);

                    if (rnd > crowbarProbability)
                    {
                        animator.SetTrigger("SwitchM9");
                        CurrentWeapon = AnimationWeapon.m9;
                    }

                    else
                    {
                        animator.SetTrigger("SwitchCr");
                        CurrentWeapon = AnimationWeapon.bar;
                    }
                }
            }

            switchInterval = 0.0f;
        }

        static void BeginShotAnimation(object obj, Vector3 direction)
        {
            animator.speed = 1.0f;

            var weapon = WeaponSystem.CurrentWeapon.Weapon;

            if (weapon == Weapon.ak)
            {
                animator.speed = SV_Status.FiringSpeed();
                animator.SetTrigger("ShotAk");
            }

            else if (weapon == Weapon.de)
            {
                animator.speed = 1.0f;
                animator.SetTrigger("ShotDe");
            }
        }

        static void BeginReloadingAnimation(object obj, bool mute)
        {
            animator.speed = SV_Status.WeaponSpeed();
            animator.SetTrigger("ReloadAk");
        }
    }
}

