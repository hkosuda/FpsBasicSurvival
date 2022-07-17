using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [RequireComponent(typeof(AudioSource))]
    public class WeaponSound : MonoBehaviour
    {
        static AudioSource audioSource;

        static AudioClip ak_shooting;
        static AudioClip ak_reloading_mag_off;
        static AudioClip ak_reloading_mag_in;
        static AudioClip ak_reloading_lever;

        static AudioClip de_shooting;
        static AudioClip m9_takeout;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            ak_shooting = GetClip("ak_shooting");
            de_shooting = GetClip("de_shooting");

            // - inner function
            static AudioClip GetClip(string clipName)
            {
                return Resources.Load<AudioClip>("Sound/Weapon/" + clipName);
            }
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
                WeaponController.Shot += PlayShootingSound;
            }

            else
            {
                WeaponController.Shot -= PlayShootingSound;
            }
        }

        static void PlayShootingSound(object obj, Vector3 direction)
        {
            if (WeaponSystem.CurrentWeapon.Weapon == Weapon.ak)
            {
                audioSource.volume = Params.volume_shooting;
                audioSource.PlayOneShot(ak_shooting);
            }

            if (WeaponSystem.CurrentWeapon.Weapon == Weapon.de)
            {
                audioSource.volume = Params.volume_shooting;
                audioSource.PlayOneShot(de_shooting);
            }
        }
    }
}

