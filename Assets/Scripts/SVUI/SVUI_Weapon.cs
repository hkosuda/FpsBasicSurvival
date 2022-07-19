using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class SVUI_Weapon : MonoBehaviour
    {
        static readonly int largeTextSize = 24;
        static readonly int smallTextSize = 20;

        static TextMeshProUGUI ammoText;
        static TextMeshProUGUI weaponText;

        static int currentAmmoInMag;
        static int currentAmmoInBag;

        static WeaponAnimator.AnimationWeapon currentWeapon;

        void Start()
        {
            weaponText = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            ammoText = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

            UpdateAmmoText();
            UpdateWeaponText();
        }

        void Update()
        {
            var _currentAmmoInMag = CurrentAmmoInMag();
            var _currentAmmoInBag = CurrentAmmoInBag();

            if (currentAmmoInMag != _currentAmmoInMag || currentAmmoInBag != _currentAmmoInBag)
            {
                currentAmmoInMag = _currentAmmoInMag;
                currentAmmoInBag = _currentAmmoInBag;

                UpdateAmmoText();
            }

            var _currentWeapon = WeaponAnimator.CurrentWeapon;

            if (currentWeapon != _currentWeapon)
            {
                currentWeapon = _currentWeapon;
                UpdateWeaponText();
            }

            // - inner function
            static int CurrentAmmoInMag()
            {
                var weapon = WeaponSystem.CurrentWeapon.Weapon;

                if (weapon == Weapon.ak)
                {
                    return AK_Availability.AmmoInMag;
                }

                if (weapon == Weapon.de)
                {
                    return DE_Availability.AmmoInMag;
                }

                return 0;
            }

            // - inner function
            static int CurrentAmmoInBag()
            {
                var weapon = WeaponSystem.CurrentWeapon.Weapon;

                if (weapon == Weapon.ak)
                {
                    return AK_Availability.AmmoInBag;
                }

                return 0;
            }
        }

        static void UpdateAmmoText()
        {
            ammoText.text = currentAmmoInMag.ToString() + " / " + currentAmmoInBag.ToString();
        }

        static void UpdateWeaponText()
        {
            if (currentWeapon == WeaponAnimator.AnimationWeapon.ak)
            {
                weaponText.fontSize = largeTextSize;
                weaponText.text = "AK-47";
                return;
            }

            if(currentWeapon == WeaponAnimator.AnimationWeapon.de)
            {
                weaponText.fontSize = largeTextSize;
                weaponText.text = "Desert Eagle";
                return;
            }

            if (currentWeapon == WeaponAnimator.AnimationWeapon.m9)
            {
                weaponText.fontSize = largeTextSize;
                weaponText.text = "M9 Bayonet";
                return;
            }

            if (currentWeapon == WeaponAnimator.AnimationWeapon.bar)
            {
                weaponText.fontSize = smallTextSize;
                weaponText.text = "Something like a crowbar";
                return;
            }

            weaponText.text = "";
        }
    }
}

