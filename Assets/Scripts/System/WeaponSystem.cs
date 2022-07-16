using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum Weapon
    {
        ak, de, m9,
    }

    public class WeaponSystem : MonoBehaviour
    {
        static public EventHandler<Weapon> WeaponChanged { get; set; }

        [SerializeField] Weapon defaultWeapon = Weapon.de;

        [SerializeField] GameObject ak;
        [SerializeField] GameObject de;
        [SerializeField] GameObject m9;

        static Dictionary<Weapon, GameObject> objectList;
        static Dictionary<Weapon, WeaponController> controllerList;

        static public WeaponController CurrentWeapon { get; private set; }

        private void Awake()
        {
            objectList = new Dictionary<Weapon, GameObject>()
            {
                { Weapon.ak, ak }, { Weapon.de, de }, { Weapon.m9, m9 }
            };

            controllerList = new Dictionary<Weapon, WeaponController>()
            {
                { Weapon.ak, new AkController(Weapon.ak) },
                { Weapon.de, new DeController(Weapon.de) },
                { Weapon.m9, new M9Controller(Weapon.m9) },
                
            };
        }

        private void Start()
        {
            foreach(var controller in controllerList.Values)
            {
                controller.Initialize();
            }

            SwitchWeapon(defaultWeapon);
            SetEvent(1);
        }

        private void OnDestroy()
        {
            foreach(var controller in controllerList.Values)
            {
                controller.Shutdown();
            }

            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                TimerSystem.Updated += UpdateController;
                TimerSystem.Updated += AcceptInput;
            }

            else
            {
                TimerSystem.Updated -= UpdateController;
                TimerSystem.Updated -= AcceptInput;
            }
        }

        static void UpdateController(object obj, float dt)
        {
            CurrentWeapon.Update(dt);
        }

        static void AcceptInput(object obj, float dt)
        {
            if (Keyconfig.CheckInput(KeyAction.ak, true))
            {
                SwitchWeapon(Weapon.ak);
            }

            else if (Keyconfig.CheckInput(KeyAction.de, true))
            {
                SwitchWeapon(Weapon.de);
            }

            else if (Keyconfig.CheckInput(KeyAction.m9, true))
            {
                SwitchWeapon(Weapon.m9);
            }
        }

        static public void SwitchWeapon(Weapon weapon)
        {
            if (CurrentWeapon != null && CurrentWeapon.Weapon == weapon) { return; }
            if (CurrentWeapon != null) { CurrentWeapon.Inactivate(); }

            CurrentWeapon = controllerList[weapon];

            CurrentWeapon.Activate();
            ActivateObject(weapon);

            WeaponChanged?.Invoke(null, weapon);

            // - inner function
            static void ActivateObject(Weapon weapon)
            {
                foreach(var w in objectList)
                {
                    if (w.Key == weapon)
                    {
                        w.Value.SetActive(true);
                    }

                    else
                    {
                        w.Value.SetActive(false);
                    }
                }
            }
        }

        
    }
}

