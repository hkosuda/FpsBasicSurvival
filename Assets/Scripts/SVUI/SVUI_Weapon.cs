using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class SVUI_Weapon : MonoBehaviour
    {
        static TextMeshProUGUI ammoText;
        static TextMeshProUGUI weaponText;

        static int currentAmmoInMag;
        static int currentAmmoInBag;

        static Weapon currentWeapon;

        void Start()
        {
            weaponText = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            ammoText = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
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

            var _currentWeapon = WeaponSystem.CurrentWeapon.Weapon;

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
            if (currentWeapon == Weapon.ak)
            {
                weaponText.text = "AK-47";
                return;
            }

            if(currentWeapon == Weapon.de)
            {
                weaponText.text = "Desert Eagle";
                return;
            }

            if (currentWeapon == Weapon.m9)
            {
                weaponText.text = "M9 Bayonet";
                return;
            }

            weaponText.text = "";
        }
    }
}

