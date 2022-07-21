using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Stream_Map : HostComponent
    {
        public override void Begin()
        {
            MapSystem.SwitchMap(MapName.ez_stream);
        }
    }
}

