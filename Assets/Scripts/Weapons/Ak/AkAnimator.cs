using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class AkAnimator : MonoBehaviour
    {
        static public EventHandler<bool> ReloadingEnd { get; set; }

        static Animator animator;

        private void Awake()
        {
            animator = gameObject.GetComponent<Animator>();
        }

        private void Start()
        {
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
                WeaponController.Shot += BeginShootingAnimation;
                WeaponController.ReloadingBegin += BeginReloadingAnimation;
            }

            else
            {
                WeaponController.Shot -= BeginShootingAnimation;
                WeaponController.ReloadingBegin -= BeginReloadingAnimation;
            }
        }

        static public void BeginChangingAnimation()
        {
            animator.SetTrigger("Change");
        }

        static void BeginShootingAnimation(object obj,Vector3 direction)
        {
            animator.SetTrigger("Shot");
        }

        static void BeginReloadingAnimation(object obj, bool mute)
        {
            animator.SetTrigger("Reload");
        }

        //
        // used in animation
        public void InvokeReloadingEnd()
        {
            ReloadingEnd?.Invoke(null, false);
        }
    }
}

