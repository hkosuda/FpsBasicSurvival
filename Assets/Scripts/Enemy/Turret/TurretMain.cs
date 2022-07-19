using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TurretMain : EnemyMain
    {
        private void Awake()
        {
            Init(EnemyType.turret, OnShot);
        }
    }
}

