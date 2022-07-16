using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    static public EventHandler<GameObject> ShootingHit { get; set; }
    static public EventHandler<Vector3> Shot { get; set; }

    static public EventHandler<bool> Empty { get; set; }
    static public EventHandler<bool> ReloadingBegin { get; set; }
}
