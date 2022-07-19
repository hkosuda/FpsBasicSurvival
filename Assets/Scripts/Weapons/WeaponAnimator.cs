using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class WeaponAnimator : MonoBehaviour
    {
        static Animator animator;

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
            }

            else if (weapon == Weapon.de)
            {
                animator.SetTrigger("SwitchDe");
            }

            else if (weapon == Weapon.m9)
            {
                animator.SetTrigger("SwitchM9");
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

