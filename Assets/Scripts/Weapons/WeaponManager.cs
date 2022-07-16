using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    akm, deagle, bayonet, karambit
}

public class WeaponManager : MonoBehaviour
{
    static GameObject myself;
    static GameObject activeWeapon;

    static Dictionary<Weapon, GameObject> _weapons;
    static Dictionary<Weapon, GameObject> _dropWeapons;

    static Dictionary<Weapon, bool> availableWeapon;

    static public Weapon ActiveWeapon { get; private set; }

    static public bool Active { get; private set; }

    //
    // used in animations
    static public void Activate()
    {
        Active = true;
    }

    static public void Deactivate()
    {
        Active = false;
    }

    public virtual void Reloaded()
    {

    }

    private void Awake()
    {
        myself = gameObject;

        _weapons = new Dictionary<Weapon, GameObject>()
        {
            { Weapon.akm, GetWeapon("akm") },
            { Weapon.deagle, GetWeapon("deagle") },
            { Weapon.bayonet, GetWeapon("bayonet") },
            { Weapon.karambit, GetWeapon("karambit") },
        };

        _dropWeapons = new Dictionary<Weapon, GameObject>()
        {
            { Weapon.akm, GetItemWeapon("ItemAKM") },
            { Weapon.deagle, GetItemWeapon("ItemDeagle") },
        };

        availableWeapon = new Dictionary<Weapon, bool>() 
        {
            { Weapon.akm, true },
            { Weapon.deagle, true },
            { Weapon.bayonet, true },
            { Weapon.karambit, true },
        };

        InstantiateWeapon(Weapon.akm);
        AkAnimator.BeginChangingAnimation();

        // - inner function
        static GameObject GetWeapon(string name)
        {
            return Resources.Load<GameObject>("Weapons/" + name);
        }

        static GameObject GetItemWeapon(string name)
        {
            return Resources.Load<GameObject>("Items/" + name);
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
            TimerSystem.Updated += UpdateMethod;
        }

        else
        {
            TimerSystem.Updated -= UpdateMethod;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        if (Keyconfig.GetKey(Keyconfig.KeyAction.gun1, true))
        {
            if (ActiveWeapon == Weapon.akm) { return; }
            if (!availableWeapon[Weapon.akm]) { return; }

            InstantiateWeapon(Weapon.akm);
            AkAnimator.BeginChangingAnimation();
            return;
        }

        if (Keyconfig.GetKey(Keyconfig.KeyAction.gun2, true))
        {
            if (ActiveWeapon == Weapon.deagle) { return; }
            if (!availableWeapon[Weapon.deagle]) { return; }

            InstantiateWeapon(Weapon.deagle);
            DeAnimator.BeginChangingAnimation();
            return;
        }

        if (Keyconfig.GetKey(Keyconfig.KeyAction.knife, true))
        {
            if(ActiveWeapon == Weapon.bayonet || ActiveWeapon == Weapon.karambit) { return; }

            KnifeRandomActivate();
            KnifeAnimator.BeginChangingAnimation();
            return;
        }

        if (Keyconfig.GetKey(Keyconfig.KeyAction.drop, true))
        {
            DropWeapon();
        }

        // - inner function
        
    }

    static void InstantiateWeapon(Weapon weaponName)
    {
        if (activeWeapon != null)
        {
            Destroy(activeWeapon);
        }

        var _weapon = _weapons[weaponName];
        var weapon = GameObject.Instantiate(_weapon);

        weapon.transform.SetParent(myself.transform);
        activeWeapon = weapon;

        ActiveWeapon = weaponName;
        Active = false;
    }

    static void DropWeapon()
    {
        if (ActiveWeapon == Weapon.bayonet || ActiveWeapon == Weapon.karambit) { return; }
        if (!_dropWeapons.ContainsKey(ActiveWeapon)) { return; }

        var origin = PlayerViewController.Self.transform.position;
        var euler = PlayerViewController.Self.transform.rotation.eulerAngles;
        var rotation = Quaternion.Euler(euler.x, euler.y, UnityEngine.Random.Range(-30.0f, -5.0f));
        var dropWeapon = GameObject.Instantiate(_dropWeapons[ActiveWeapon], origin, rotation);

        availableWeapon[ActiveWeapon] = false;

        var velocity = Utility.GetViewVector(5.0f, r: 0.2f) + PlayerController.Rb.velocity;
        dropWeapon.GetComponent<Rigidbody>().velocity = velocity;

        AddAction(dropWeapon);

        

        ActivateOtherWeapon();

        // - inner function
        static void ActivateOtherWeapon()
        {
            if (ActiveWeapon == Weapon.akm)
            {
                if (availableWeapon[Weapon.deagle])
                {
                    InstantiateWeapon(Weapon.deagle);
                    DeAnimator.BeginChangingAnimation();
                }
                else
                {
                    KnifeRandomActivate();
                    KnifeAnimator.BeginChangingAnimation();
                }
            }

            else
            {
                if (availableWeapon[Weapon.akm])
                {
                    InstantiateWeapon(Weapon.akm);
                    AkAnimator.BeginChangingAnimation();
                }

                else
                {
                    KnifeRandomActivate();
                    KnifeAnimator.BeginChangingAnimation();
                }
            }
        }

        static void AddAction(GameObject dropWeapon)
        {
            var drop = dropWeapon.transform.GetChild(0).gameObject.GetComponent<DropWeapon>();

            if (ActiveWeapon == Weapon.akm)
            {
                drop.Setup(ActionOfDroppedAKM, Floats.Get(Floats.Item.item_drop_weapon_inactive_time));
            }
            
            if (ActiveWeapon == Weapon.deagle)
            {
                drop.Setup(ActionOfDroppedDe, Floats.Get(Floats.Item.item_drop_weapon_inactive_time));
            }
        }
    }

    static void KnifeRandomActivate()
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        var value = UnityEngine.Random.Range(0.0f, 1.0f);

        if (value < 0.5f)
        {
            InstantiateWeapon(Weapon.karambit);
        }

        else
        {
            InstantiateWeapon(Weapon.bayonet);
        }
    }


    // drop weapon method
    static bool ActionOfDroppedAKM()
    {
        if (!availableWeapon[Weapon.akm])
        {
            availableWeapon[Weapon.akm] = true;
            return true;
        }

        return false;
    }

    static bool ActionOfDroppedDe()
    {
        if (!availableWeapon[Weapon.deagle])
        {
            availableWeapon[Weapon.deagle] = true;
            return true;
        }

        return false;
    }
}
