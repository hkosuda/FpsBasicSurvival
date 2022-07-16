using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SVUI_Ammo : MonoBehaviour
{
    TextMeshProUGUI weaponText;
    TextMeshProUGUI ammoText;

    int currentAmmoInMag;
    int currentAmmoInBag;

    Weapon currentWeapon;

    private void Awake()
    {
        weaponText = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        ammoText = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
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

        }

        else
        {

        }
    }

    void Update()
    {
        if (SV_StatusAdmin.StatusList == null) { return; }

        var ammoInMag = GetCurrentAmmoInMag(WeaponManager.ActiveWeapon);
        var ammoInBag = GetCurrentAmmoInBag(WeaponManager.ActiveWeapon);

        if (currentWeapon == WeaponManager.ActiveWeapon)
        {
            if (currentAmmoInMag == ammoInMag && currentAmmoInBag == ammoInBag) 
            {
                return;
            }
        }

        currentWeapon = WeaponManager.ActiveWeapon;
        currentAmmoInMag = ammoInMag;
        currentAmmoInBag = ammoInBag;

        UpdateText();
    }

    void UpdateText()
    {
        weaponText.text = GetWeaponText(currentWeapon);
        ammoText.text = currentAmmoInMag.ToString() + " / " + currentAmmoInBag.ToString();

        // - inner function
        static string GetWeaponText(Weapon weapon)
        {
            if (weapon == Weapon.akm)
            {
                return "AKM";
            }

            if (weapon == Weapon.deagle)
            {
                return "Desert Eagle";
            }

            if (weapon == Weapon.bayonet)
            {
                return "Bayonet Knife";
            }

            if (weapon == Weapon.karambit)
            {
                return "Karambit Knife";
            }

            return "";
        }
    }

    int GetCurrentAmmoInMag(Weapon weapon)
    {
        if (weapon == Weapon.akm)
        {
            return AkController.AmmoInMag;
        }

        if (weapon == Weapon.deagle)
        {
            return DeController.AmmoInMag;
        }

        else
        {
            return 0;
        }
    }

    int GetCurrentAmmoInBag(Weapon weapon)
    {
        if (weapon == Weapon.akm)
        {
            return AkController.AmmoInBag;
        }

        if (weapon == Weapon.deagle)
        {
            return DeController.AmmoInBag;
        }

        else
        {
            return 0;
        }
    }
}