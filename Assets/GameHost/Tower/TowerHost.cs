using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TowerHost : GameHost
    {
        public TowerHost(HostName hostName) : base(hostName)
        {
            componentList = new List<HostComponent>()
            {
                new BHop_Command(),
                new Tower_Map(),
            };
        }
    }
}

