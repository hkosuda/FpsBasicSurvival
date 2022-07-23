using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Square_Map : HostComponent
    {
        public override void Begin()
        {
            MapSystem.SwitchMap(MapName.ez_square);
        }
    }
}

