using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    Animator animator;
    WeaponController weaponController;

    void Awake()
    {
        animator = GetComponent<Animator>();
        weaponController = gameObject.GetComponent<WeaponController>();
    }

    //
    // used in animation
    public void WeaponActivate()
    {
        WeaponManager.Activate();
    }

    public void WeaponDeactivate()
    {
        WeaponManager.Deactivate();
    }
}
