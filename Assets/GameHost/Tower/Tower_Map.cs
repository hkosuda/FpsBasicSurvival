using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Tower_Map : HostComponent
    {
        public override void Begin()
        {
            MapSystem.SwitchMap(MapName.ez_tower);
        }
    }
}

