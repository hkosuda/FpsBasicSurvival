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
        static AudioClip ak_mag_off;
        static AudioClip ak_mag_in;
        static AudioClip ak_lever;

        static AudioClip de_shooting;
        static AudioClip de_takeout;

        static AudioClip m9_takeout;

        static AudioClip empty;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            ak_shooting = GetClip("ak_shooting");
            ak_mag_off = GetClip("ak_mag_off");
            ak_mag_in = GetClip("ak_mag_in");
            ak_lever = GetClip("ak_lever");

            de_shooting = GetClip("de_shooting");
            de_takeout = GetClip("de_takeout");

            m9_takeout = GetClip("m9_takeout");

            empty = GetClip("empty");

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
                WeaponController.Empty += PlayEmptySound;
            }

            else
            {
                WeaponController.Shot -= PlayShootingSound;
                WeaponController.Empty -= PlayEmptySound;
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

        static void PlayEmptySound(object obj, bool mute)
        {
            audioSource.volume = Params.volume_shooting;
            audioSource.PlayOneShot(empty);
        }

        //
        // used in animation

        void PlayMagOffSound()
        {
            audioSource.volume = Params.volume_shooting;
            audioSource.PlayOneShot(ak_mag_off);
        }

        void PlayMagInSound()
        {
            audioSource.volume = Params.volume_shooting;
            audioSource.PlayOneShot(ak_mag_in);
        }

        void PlayLeverSound()
        {
            audioSource.volume = Params.volume_shooting;
            audioSource.PlayOneShot(ak_lever);
        }

        void PlayDeTakeoutSound()
        {
            // no sound effect
        }

        void PlayM9TakeoutSound()
        {
            audioSource.volume = 0.7f;
            audioSource.PlayOneShot(m9_takeout);
        }
    }
}

