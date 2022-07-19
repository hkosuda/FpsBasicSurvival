using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class WeaponAnimator : MonoBehaviour
    {
        public enum AnimationWeapon
        {
            ak, de, m9, bar,
        }

        static Animator animator;
        static public AnimationWeapon CurrentWeapon { get; private set; }

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
                WeaponSystem.WeaponChanged += BeginTakingoutAnimation;
                WeaponController.Shot += BeginShotAnimation;

                AK_Reload.ReloadingBegin += BeginReloadingAnimation;
            }

            else
            {
                WeaponSystem.WeaponChanged -= BeginTakingoutAnimation;
                WeaponController.Shot -= BeginShotAnimation;

                AK_Reload.ReloadingBegin -= BeginReloadingAnimation;
            }
        }

        static void BeginTakingoutAnimation(object obj, Weapon weapon)
        {
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
                //animator.SetTrigger("SwitchM9");
                animator.SetTrigger("SwitchCr");
                CurrentWeapon = AnimationWeapon.bar;
            }
        }

        static void BeginShotAnimation(object obj, Vector3 direction)
        {
            var weapon = WeaponSystem.CurrentWeapon.Weapon;

            if (weapon == Weapon.ak)
            {
                animator.SetTrigger("ShotAk");
            }

            else if (weapon == Weapon.de)
            {
                animator.SetTrigger("ShotDe");
            }
        }

        static void BeginReloadingAnimation(object obj, bool mute)
        {
            animator.SetTrigger("ReloadAk");
        }
    }
}

